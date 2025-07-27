using Godot;

namespace Kiwijam2025.Scripts.Items;

public partial class TV : Item
{
    public TV()
    {
        // set the sin, effect, and point bases
        Sin = Sin.Sloth;
        Effect = new TVEffect(); // We have this as a field so that effects can be swapped out at runtime
        PointGen.Base = 5;

    }
}

public partial class TVEffect : ItemEffect
{
    public override void Apply(Item I)
    {
        for (var x = -1; x <= 1; x++)
        {
            for (var y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;
                var item = I.GetRelative(new Vector2I(x, y));
                if (item != null) PlayerWallet.Points += 5;
            }
        }
    }
}