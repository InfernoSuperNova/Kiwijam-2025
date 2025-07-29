using Godot;

namespace Kiwijam2025.Scripts.Items;

public partial class Peace : Item
{
    public Vector2I GridPositionToSet;
    public Peace()
    {
        // set the sin, effect, and point bases
        Sin = Sin.Wrath;
        Effect = new PeaceEffect(); // We have this as a field so that effects can be swapped out at runtime
        PointGen.Base = 5;

    }
    public override void _Ready()
    {
        GridPosition = GridPositionToSet;
        base._Ready();
    }
}

public partial class PeaceEffect : ItemEffect
{
    public override void Apply(Item I)
    {
        var left = I.GetRelative(new Vector2I(-1, 0));
        if (left != null) left.Effect = null;
        var right = I.GetRelative(new Vector2I(1, 0));
        if (right != null) right.Effect = null;
        
    }
}