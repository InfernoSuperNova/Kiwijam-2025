using System;

namespace Kiwijam2025.Scripts.Items;

public partial class Bank : Item
{
    public Bank()
    {
        // set the sin, effect, and point bases
        Sin = Sin.Greed;
        Effect = new BankEffect(); // We have this as a field so that effects can be swapped out at runtime
        PointGen.Base = 50;
    }
    
    
}

public partial class BankEffect : ItemEffect
{
    private float CollapseChance = 0;
    private Random rng = new();
    public override void Apply(Item I)
    {
        float roll = rng.NextSingle();

        if (roll < CollapseChance)
        {
            I.Destroy();
            
            //var collapsedBank = Game.I.fileToPackedScene[itemName];
        }
        
        CollapseChance += 0.1f;
    }
}