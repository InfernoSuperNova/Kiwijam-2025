namespace Kiwijam2025.Scripts.Items;

public partial class Book : Item
{
    public Book()
    {
        Sin = Sin.Sloth;
        Effect = new BookEffect();
        PointGen.Base = 0;
        // TODO: Subtract SP from the player's wallet
        
    }
}

public partial class BookEffect : ItemEffect
{
    public override void Apply(Item i)
    {
        var top = i.GetRelative(0, -1);
        var bottom = i.GetRelative(0, 1);
        var left = i.GetRelative(-1, 0);
        var right = i.GetRelative(1, 0);
        if (top != null && bottom != null && left != null && right != null)
        {
            top.PointGen.Mul *= 2; 
            bottom.PointGen.Mul *= 2;
            left.PointGen.Mul *= 2;
            right.PointGen.Mul *= 2;
        }
    }
}