using Godot;
using System;

public partial class ShopItem : Control
{
	[Export]
	public long cost = 1;
	[Export]
	string name = "sword";
	RichTextLabel label;
	Button button;

	public override void _Ready()
	{
		base._Ready();
		label = GetNode<RichTextLabel>("VBoxContainer/Label");
		button = GetNode<Button>("VBoxContainer/Item");
		label.Text = "Cost: " + cost;
		button.Text = name;
	}

}
