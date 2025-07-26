#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Godot;

namespace Kiwijam2025;

public enum Sin
{
    Pride,
    Greed,
    Lust,
    Envy,
    Gluttony,
    Wrath,
    Sloth
}

public struct PointGen()
{
    public float Base = 0;
    public float AddPreMul = 0;
    public float Mul = 1;
    public float AddPostMul = 0;

    public float Value => (Base + AddPreMul) * Mul + AddPostMul;
    public static implicit operator float(PointGen p) => p.Value;
}

#region ItemBase
public abstract partial class Item : Node3D
{
    #region static
    // List of all active item instances
    public static readonly List<Item> Items = new();

    // Simple grid: position to item
    public static readonly Dictionary<Vector2I, Item> ItemGrid = new();

    // List of all known item types
    public static readonly List<Type> AllItemTypes;

    static Item()
    {
        // Populates the known item type list with all classes inheriting directly from Item at runtime
        AllItemTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => typeof(Item).IsAssignableFrom(t) && !t.IsAbstract && t != typeof(Item))
            .ToList();
    }
    #endregion

    #region instanced
    #region events
    public override void _EnterTree()
    {
        Items.Add(this);
        ItemGrid[GridPosition] = this;
    }

    public override void _ExitTree()
    {
        Items.Remove(this);
        ItemGrid.Remove(GridPosition);
    }

    public virtual void _ActivateItem()
    {
        GD.Print($"{GetType().Name} placed at {GridPosition}");
        Effect.Apply(this);
    }
    #endregion

    #region members
    // TODO: Setup model type
    internal object _model;

    private Type? _cachedType;
    public Type Type => _cachedType ??= GetType();

    public Sin Sin;

    public Vector2I GridPosition;

    public ItemEffect Effect;



    
    #region pointgen

    public PointGen sp;
    public PointGen mp;
    public PointGen lp;

    #endregion
    
    #endregion

    #region methods

    // Returns the item relative to this one in grid coordinates, or null if none
    public Item? GetRelative(Vector2I offset)
    {
        ItemGrid.TryGetValue(GridPosition + offset, out var item);
        return item;
    }
    #endregion
    #endregion
}

public partial class ItemEffect
{
    public virtual void Apply(Item I) {}
}
#endregion
#region ItemTemplate
public partial class ItemTemplate : Item
{
    public ItemTemplate()
    {
        // set the sin, effect, and point bases
        Sin = Sin.Pride;
        Effect = new ItemTemplateEffect(); // We have this as a field so that effects can be swapped out at runtime
        sp.Base = 0;
        mp.Base = 0;
        lp.Base = 0;
        
    }

    // Hook into event functions to trigger the effect
    public override void _ActivateItem()
    {
        base._ActivateItem();
        Effect.Apply(this);
    }
}

public partial class ItemTemplateEffect : ItemEffect
{
    public override void Apply(Item I)
    {
        // Put your effect functionality in here
    }
}
#endregion
#region Sword
public partial class Sword : Item
{
    public Sword()
    {
        Sin = Sin.Wrath;
        Effect = new SwordEffect();
        //TODO: Fill these out
        sp.Base = 0;
        mp.Base = 0;
        lp.Base = 0;
        
    }

    public override void _ActivateItem()
    {
        base._ActivateItem();
        Effect.Apply(this);
    }
}

public partial class SwordEffect : ItemEffect
{
    public override void Apply(Item i)
    {
        base.Apply(i);

        var right = i.GetRelative(new Vector2I(1, 0));
        if (right == null)
            return;

        switch (right.Sin)
        {
            case Sin.Pride:
                GD.Print("Sword effect: Set all points to 20 (placeholder, set player wallet points)");
                break;

            case Sin.Greed:
                GD.Print("Sword effect: Give 5 MP (placeholder, set player wallet points)");
                break;

            case Sin.Envy:
                GD.Print("Sword effect: Copy effect of item on the right");
                Type effectType = right.Effect.GetType();
                i.Effect = (ItemEffect)Activator.CreateInstance(effectType);
                break;

            case Sin.Wrath:
                GD.Print("Sword effect: Becomes Peace (placeholder)");
                break;

            default:
                GD.Print($"Sword effect: No special effect for {right.Sin}");
                break;
        } 
    }
}
#endregion
#region Pan

public partial class Pan : Item
{
    public Pan()
    {
        Sin = Sin.Wrath;
        Effect = new PanEffect();
        sp.Base = 5;
        mp.Base = 0;
        lp.Base = 0;
    }
    public override void _ActivateItem()
    {
        base._ActivateItem();
        Effect.Apply(this);
    }
}

public partial class PanEffect : ItemEffect
{
    public override void Apply(Item i)
    {
        base.Apply(i);
        
        
        // Halves the point output if items to the left and right
        // Adds half of the items to the left and right to itself
        var left  = i.GetRelative(new Vector2I(-1, 0));
        if (left != null)
        {
            i.sp.AddPreMul += left.sp;
            i.mp.AddPreMul += left.mp;
            i.lp.AddPreMul += left.lp;

            left.sp.Mul *= 0.5f;
            left.mp.Mul *= 0.5f;
            left.lp.Mul *= 0.5f;
        }
        var right = i.GetRelative(new Vector2I(1, 0));
        if (right != null)
        {
            i.sp.AddPreMul  += right.sp;
            i.mp.AddPreMul  += right.mp;
            i.lp.AddPreMul += right.lp;
            
            right.sp.Mul *= 0.5f;
            right.mp.Mul *= 0.5f;
            right.lp.Mul *= 0.5f;
        }
        
    }
}
#endregion
#region Boxing Glove

public partial class BoxingGlove : Item
{
    public BoxingGlove()
    {
        Sin = Sin.Wrath;
        Effect = new BoxingGloveEffect();
        sp.Base = 0;
        mp.Base = 0;
        lp.Base = 0;
        // TODO: Subtract SP from the player's wallet
        
    }
}

public partial class BoxingGloveEffect : ItemEffect
{
    public override void Apply(Item i)
    {
    }
}
#endregion
#region Guillotine
public partial class Guillotine : Item
{
    public Guillotine()
    {
        // set the sin, effect, and point bases
        Sin = Sin.Wrath;
        Effect = new GuillotineEffect(); // We have this as a field so that effects can be swapped out at runtime
        sp.Base = 0;
        mp.Base = 0;
        lp.Base = 0;
        
    }

    // Hook into event functions to trigger the effect
    public override void _ActivateItem()
    {
        base._ActivateItem();
        Effect.Apply(this);
    }
}

public partial class GuillotineEffect : ItemEffect
{
    public override void Apply(Item I)
    {
        // Put your effect functionality in here
    }
}
#endregion