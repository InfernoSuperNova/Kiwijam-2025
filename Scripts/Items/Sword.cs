    
using System;
using Godot;

namespace Kiwijam2025.Scripts.Items;

public partial class Sword : Item
{
    public Sword()
    {
        Sin = Sin.Wrath;
        Effect = new SwordEffect();
        //TODO: Fill these out
        PointGen.Base = 1;
        PointGen.Mul = 1;

    }
}


public partial class SwordEffect : ItemEffect
{
    public override void Apply(Item i)
    {
        base.Apply(i);

        var right = i.GetRelative(new Vector2I(1, 0));
        if (right == null)
            return;

        switch (right.Sin)
        {
            case Sin.Pride:
                GD.Print("Sword effect: Set all points to 20 (placeholder, set player wallet points)");
                //PlayerWallet.Points = 20;
                right.PointGen.Base = 20;
                break;

            case Sin.Greed:
                GD.Print("Sword effect: Give 5 MP (placeholder, set player wallet points)");
                PlayerWallet.Points += 5;
                break;

            case Sin.Envy:
                GD.Print("Sword effect: Copy effect of item on the right");
                Type effectType = right.Effect.GetType();
                ItemEffect effect = (ItemEffect)Activator.CreateInstance(effectType);
                effect.Apply(i);
                break;

            case Sin.Wrath:
                GD.Print("Sword effect: Becomes Peace (placeholder)");
                break;

            default:
                GD.Print($"Sword effect: No special effect for {i.Sin}");
                break;
        } 
        right.QueueFree();
    }
}
