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

public struct PointGen
{
    public double Base;
    public double AddPreMul;
    public double Mul;
    public double AddPostMul;

    // Explicit parameterless constructor
    public PointGen()
    {
        Base = 0;
        AddPreMul = 0;
        Mul = 1;
        AddPostMul = 0;
    }

    public double Value => (Base + AddPreMul) * Mul + AddPostMul;
    public static implicit operator double(PointGen p) => p.Value;
    public static explicit operator long(PointGen p) => (long)p.Value;
}


#region ItemBase
public abstract partial class Item : Node3D
{
    #region static
    public static IReadOnlyCollection<Item> All => Items;
    internal static readonly List<Item> Items = [];

    /// <summary>
    /// External access to grid. Cannot be used to change the positions. Please set item positions instead.
    /// </summary>
    public static IReadOnlyDictionary<Vector2I, Item> Grid => ItemGrid;
    /// <summary>
    /// INTERNAL item grid only. DO NOT expose externally.
    /// </summary>
    internal static readonly Dictionary<Vector2I, Item> ItemGrid = [];

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
        CallDeferred(nameof(DeferredAdd));
        base._EnterTree();
    }

    private void DeferredAdd()
    {
        Items.Add(this);
    }

    public override void _ExitTree()
    {
        Items.Remove(this);
        ItemGrid.Remove(_gridPosition);
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

    


    private Vector2I _gridPosition = new Vector2I(-1, -1);
    
    
    /// <summary>
    /// The primary entrypoint for setting position on the grid. Also updates the ItemGrid dictionary.
    /// </summary>
    public Vector2I GridPosition
    {
        get => _gridPosition;
        set
        {
            
            if (_gridPosition == value) return; // no change, no action

            ItemGrid.Remove(_gridPosition);

            _gridPosition = value;

            // If the key already exists, overwrite it to avoid exception
            ItemGrid[_gridPosition] = this;
        }
    }

    public ItemEffect Effect;



    
    #region pointgen
    
    public PointGen PointGen;
    public PointGen OriginalPointGen;

    #endregion
    
    #endregion

    #region methods
    // Returns the item relative to this one in grid coordinates, or null if none
    public Item? GetRelative(Vector2I offset)
    {
        ItemGrid.TryGetValue(GridPosition + offset, out var item);
        return item;
    }

    public Item? GetRelative(int x, int y)
    {
        var offset = new Vector2I(x, y);
        return GetRelative(offset);
    }

    public Item()
    {
        PointGen.Mul = 1;
    }

    public void Destroy()
    {
        QueueFree();
        // Game.I.
    }

    public bool HasSetup = false;
    public virtual void Setup() { }
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

