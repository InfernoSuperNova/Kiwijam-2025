using Godot;

namespace Kiwijam2025.Scripts.Items;

public partial class Guillotine : Item
{
    public Guillotine()
    {
        // set the sin, effect, and point bases
        Sin = Sin.Wrath;
        Effect = new GuillotineEffect(); // We have this as a field so that effects can be swapped out at runtime
        PointGen.Base = 1;
        PointGen.Mul = 1;
    }
}

public partial class GuillotineEffect : ItemEffect
{
    public override void Apply(Item I)
    {
        for (int x = -1; x <= 1; x++)
        {
            var item = I.GetRelative(new Vector2I(x, 1));
            if (item != null)
            {
                item.QueueFree();
                PlayerWallet.Points += 20;
            }
        }
    }
}