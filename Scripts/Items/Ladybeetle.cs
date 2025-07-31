using System;
using Godot;

namespace Kiwijam2025.Scripts.Items;

public partial class Ladybeetle : Item
{
	public Ladybeetle()
	{
		// set the sin, effect, and point bases
		Sin = Sin.Lust;
		Effect = new LadyBeetleEffect(); // We have this as a field so that effects can be swapped out at runtime

	}
}

public partial class LadyBeetleEffect : ItemEffect
{
	public override void Apply(Item I)
	{
		var random = new Random();
		for (var x = -1; x <= 1; x++)
		{
			for (var y = -1; y <= 1; y++)
			{
				float mul = random.NextSingle();
				if (x == 0 && y == 0) continue;
				var item = I.GetRelative(new Vector2I(x, y));
				if (item != null) item.PointGen.Mul *= mul * 2.5;
			}
		}
	}
}
