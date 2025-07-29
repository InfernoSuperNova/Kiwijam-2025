using Godot;

namespace Kiwijam2025.Scripts.Items;

public partial class Potion : Item
{
    public Potion()
    {
        // set the sin, effect, and point bases
        Sin = Sin.Envy;
        Effect = new PotionEffect(); // We have this as a field so that effects can be swapped out at runtime
        PointGen.Base = 5;

    }
}

public partial class PotionEffect : ItemEffect
{
    public override void Apply(Item I)
    {
        for (var x = -1; x <= 1; x++)
        {
            for (var y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;
                var item = I.GetRelative(new Vector2I(x, y));
                if (item != null) item.PointGen.Mul *= 3;
            }
        }
        I.Destroy();
    }
}