using Godot;
using System;

public abstract partial class TempItem : Node3D
{
    [Export]
    public long basePoints = 1;

    public abstract long GeneratePoints();
}
