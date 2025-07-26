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

    public virtual void _PlaceItem()
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

#region Sword
public partial class Sword : Item
{
    public Sword()
    {
        Sin = Sin.Pride;
        Effect = new SwordEffect();
    }

    public override void _PlaceItem()
    {
        base._PlaceItem();
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
                GD.Print("Sword effect: Set all points to 20 (placeholder)");
                break;

            case Sin.Greed:
                GD.Print("Sword effect: Give 5 MP (placeholder)");
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
