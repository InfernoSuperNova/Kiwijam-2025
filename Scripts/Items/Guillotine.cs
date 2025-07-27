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
        // Put your effect functionality in here
    }
}