namespace Kiwijam2025.Scripts.Items;


public partial class PileOfCoins : Item
{
    public PileOfCoins()
    {
        // set the sin, effect, and point bases
        Sin = Sin.Greed;
        Effect = new PileOfCoinsEffect(); // We have this as a field so that effects can be swapped out at runtime
        PointGen.Base = 25;

    }
}

public partial class PileOfCoinsEffect : ItemEffect
{
    public override void Apply(Item I)
    {
        // Put your effect functionality in here
    }
}
