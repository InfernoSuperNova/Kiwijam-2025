using Godot;

namespace Kiwijam2025.Scripts.Items;

public partial class Medal : Item
{
    public Medal()
    {
        // set the sin, effect, and point bases
        Sin = Sin.Pride;
        Effect = new MedalEffect(); // We have this as a field so that effects can be swapped out at runtime

    }
}

public partial class MedalEffect : ItemEffect
{
    public override void Apply(Item I)
    {
        var left = I.GetRelative(new Vector2I(-1, 0));
        if (left != null)
        {
            left.PointGen.Mul *= -1;
        }
        var right = I.GetRelative(new Vector2I(1, 0));
        if (right != null)
        {
            right.PointGen.Mul *= 2;
        }
        
    }
}