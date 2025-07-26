using Godot;
using Kiwijam2025.Debug;

namespace Kiwijam2025;

public partial class TestScript : Node
{
	public TestScript()
	{
		
		
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		DebugDraw.Line(Vector3.Zero, Vector3.Up * 100, Colors.White);
	}
}
