using Godot;

namespace Kiwijam2025.Scripts.Items;

public partial class GraduationHat : Item
{
    public GraduationHat()
    {
        // set the sin, effect, and point bases
        Sin = Sin.Pride;
        Effect = new GraduationHatEffect(); // We have this as a field so that effects can be swapped out at runtime

    }
}

public partial class GraduationHatEffect : ItemEffect
{
    public override void Apply(Item I)
    {
        var other = I.GetRelative(new Vector2I(0, 1));
        if (other != null) other.PointGen.Mul *= 3;
    }
}