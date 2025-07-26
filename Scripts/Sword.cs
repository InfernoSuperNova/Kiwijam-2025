using Godot;
using System;

public partial class Sword : TempItem
{
    public override long GeneratePoints()
    {
        return basePoints;
    }
}
