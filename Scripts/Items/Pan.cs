
using Godot;

namespace Kiwijam2025.Scripts.Items;

public partial class Pan : Item
{
    public Pan()
    {
        Sin = Sin.Wrath;
        Effect = new PanEffect();
        PointGen.Base = 5;
    }
}

public partial class PanEffect : ItemEffect
{
    public override void Apply(Item i)
    {
        base.Apply(i);
        
        
        // Halves the point output if items to the left and right
        // Adds half of the items to the left and right to itself
        var left  = i.GetRelative(new Vector2I(-1, 0));
        if (left != null)
        {
            i.PointGen.AddPreMul += left.PointGen;
            
            left.PointGen.Mul *= 0.5;
        }
        var right = i.GetRelative(new Vector2I(1, 0));
        if (right != null)
        {
            i.PointGen.AddPreMul += right.PointGen;
            
            right.PointGen.Mul *= 0.5;
        }
        
    }
}
