#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Godot;

namespace Kiwijam2025.Scripts.Items;

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
    public double Base = 0;
    public double AddPreMul = 0;
    public double Mul = 1;
    public double AddPostMul = 0;

    public double Value => (Base + AddPreMul) * Mul + AddPostMul;
    public static implicit operator double(PointGen p) => p.Value;
    public static explicit operator long(PointGen p) => (long)p.Value;
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
        GD.Print($"{GetType().Name} activated at {GridPosition}");
        Effect.Apply(this);
    }

    public virtual long _GeneratePoints()
    {
        return (long)PointGen;
    }
    #endregion

    #region members

    private Type? _cachedType;
    public Type Type => _cachedType ??= GetType();

    public Sin Sin;

    public Vector2I GridPosition;

    public ItemEffect Effect;



    
    #region pointgen
    
    public PointGen PointGen;

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
/*
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
}

public partial class ItemTemplateEffect : ItemEffect
{
    public override void Apply(Item I)
    {
        // Put your effect functionality in here
    }
}
*/
#endregion

