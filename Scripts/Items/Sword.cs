    
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
                GD.Print("Sword effect: Permanently add 3 points to sword base)");
                i.OriginalPointGen.Base += 3;
                break;

            case Sin.Greed:
                GD.Print("Sword effect: Permanently add base points of item on right to wrath item on left");
                var left = i.GetRelative(new Vector2I(-1, 0));
                if (left == null || left.Sin != Sin.Wrath)
                    break;
                left.OriginalPointGen.Base += right.PointGen.Base;
                break;

            case Sin.Envy:
                GD.Print("Sword effect: Copy effect of item on the right");
                Type effectType = right.Effect.GetType();
                ItemEffect effect = (ItemEffect)Activator.CreateInstance(effectType);
                effect.Apply(i);
                break;

            case Sin.Wrath:
                GD.Print("Sword effect: Becomes Peace");
                var peacePrefab = Game.I.fileToPackedScene["Peace"];
                var peace = (Peace)peacePrefab.Instantiate();
                peace.GridPositionToSet = right.GridPosition;
                right.GetParent().AddChild(peace);
                break;

            default:
                GD.Print($"Sword effect: No special effect for {i.Sin}");
                break;
        } 
        right.Destroy();
    }
}
