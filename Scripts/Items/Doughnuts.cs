using Godot;

namespace Kiwijam2025.Scripts.Items;

public partial class Doughnuts : Item
{
    public Doughnuts()
    {
        // set the sin, effect, and point bases
        Sin = Sin.Gluttony;
        Effect = new DoughnutsEffect(); // We have this as a field so that effects can be swapped out at runtime

    }
}

public partial class DoughnutsEffect : ItemEffect
{
    public override void Apply(Item I)
    {
        for (int x = -2; x <= 2; x++)
        {
            for (int y = -2; y <= 2; y++)
            {
                var item = I.GetRelative(new Vector2I(x, y));
                if (item != null)
                {
                    item.PointGen.AddPreMul += 2;
                    item.PointGen.Mul *= 1.05;
                }
            }
        }
    }
}