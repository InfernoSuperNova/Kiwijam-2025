using Godot;

namespace Kiwijam2025.Scripts.Items;

public partial class Fries : Item
{
    public Fries()
    {
        // set the sin, effect, and point bases
        Sin = Sin.Gluttony;
        Effect = new FriesEffect(); // We have this as a field so that effects can be swapped out at runtime

    }
}

public partial class FriesEffect : ItemEffect
{
    public override void Apply(Item I)
    {
        var other = I.GetRelative(new Vector2I(-1, 0));
        if (other != null) other.PointGen.AddPreMul = 20;
    }
}