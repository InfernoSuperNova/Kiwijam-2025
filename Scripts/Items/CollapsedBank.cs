namespace Kiwijam2025.Scripts.Items;

public partial class CollapsedBank : Item
{
    public CollapsedBank()
    {
        // set the sin, effect, and point bases
        Sin = Sin.Greed;
        Effect = new CollapsedBankEffect(); // We have this as a field so that effects can be swapped out at runtime
        PointGen.Base = -7;
    }
}

public partial class CollapsedBankEffect : ItemEffect
{
    public override void Apply(Item I)
    {
        
    }
}