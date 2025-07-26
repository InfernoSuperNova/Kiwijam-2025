using Godot;
using Kiwijam2025.Debug;
using System;
using System.Collections.Generic;

public partial class Game : Node3D
{
    public static bool isBuying = false;
    public List<StaticBody3D> itemSlots = [];
    Camera3D camera;
    StaticBody3D selectedSlot;

    public override void _Ready()
    {
        base._Ready();
        Node3D activeItemSlots = GetNode<Node3D>("ItemSlots/Active");
        foreach (Node node in activeItemSlots.GetChildren())
        {
            if(node is StaticBody3D itemSlot)
            {
                itemSlots.Add(itemSlot);
            }
            
        }
        camera = GetNode<Camera3D>("Camera3D");

    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        if(@event is InputEventMouseMotion mouseMotionEvent)
        {
            OnMouseMove(mouseMotionEvent);
        }
    }

    public void OnMouseMove(InputEventMouseMotion mouseMotionEvent)
    {
        Vector3 start = camera.ProjectRayOrigin(mouseMotionEvent.Position);
        Vector3 end = camera.ProjectPosition(mouseMotionEvent.Position, 10);
        
        PhysicsRayQueryParameters3D parameters = new();
        parameters.From = start;
        parameters.To = end;
        Godot.Collections.Dictionary result = GetWorld3D().DirectSpaceState.IntersectRay(parameters);
        if (result.Count > 0)
        {
            StaticBody3D body = (StaticBody3D)result["collider"];
            if(body.GetChildren().Count < 5)
            {
                selectedSlot = body;
            } else
            {
                selectedSlot = null;
            }
        }
    }

    public override void _Process(double deltaTime)
    {
        foreach(StaticBody3D body in itemSlots)
        {
            Node3D node = (Node3D)body.FindChild("Item_Slot");
            
            node.Show();
            node = (Node3D)body.FindChild("Item_Slot_Red");
            node.Hide();
            GD.Print(selectedSlot);

            if (selectedSlot != null)
            {
                node = (Node3D)selectedSlot.FindChild("Item_Slot");
                
                node.Hide();
                node = (Node3D)selectedSlot.FindChild("Item_Slot_Red");
                GD.Print(node);
                node.Show();
            }
        }
    }
}
