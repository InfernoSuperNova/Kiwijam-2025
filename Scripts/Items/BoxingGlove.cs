namespace Kiwijam2025.Scripts.Items;


public partial class BoxingGlove : Item
{
    public BoxingGlove()
    {
        Sin = Sin.Wrath;
        Effect = new BoxingGloveEffect();
        PointGen.Base = 0;
        // TODO: Subtract SP from the player's wallet
        
    }
}

public partial class BoxingGloveEffect : ItemEffect
{
    public override void Apply(Item i)
    {
    }
}