using System.Numerics;
using Godot;

namespace Kiwijam2025.Scripts.Items;

public partial class CollapsedBank : Item
{
    public Vector2I GridPositionToSet;
    public CollapsedBank()
    {
        // set the sin, effect, and point bases
        Sin = Sin.Greed;
        Effect = new CollapsedBankEffect(); // We have this as a field so that effects can be swapped out at runtime
        PointGen.Base = -7;
    }

    public override void _Ready()
    {
        GridPosition = GridPositionToSet;
        base._Ready();
    }
}

public partial class CollapsedBankEffect : ItemEffect
{
    public override void Apply(Item I)
    {
        
    }
}