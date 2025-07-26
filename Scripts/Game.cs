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
    RichTextLabel activationText;
    long activations = 100;
    RichTextLabel pointsText;
    long points = 10;

    public override void _Ready()
    {
        base._Ready();
        Node3D activeItemSlots = GetNode<Node3D>("ItemSlots/Active");
        activationText = GetNode<RichTextLabel>("Control/ColorRect/Activations");
        activationText.Text = "Activations Left: " + activations;
        pointsText = GetNode<RichTextLabel>("Control/ColorRect/Points");
        pointsText.Text = "Points: " + points;
        foreach (Node node in activeItemSlots.GetChildren())
        {
            if(node is StaticBody3D itemSlot)
            {
                itemSlots.Add(itemSlot);
            }
            
        }
        camera = GetNode<Camera3D>("Camera3D");

    }

    public void ActivateButton()
    {
        activations--;
        activationText.Text = "Activations Left: " + activations;
        points++;
        pointsText.Text = "Points: " + points;
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        if(@event is InputEventMouseMotion mouseMotionEvent)
        {
            OnMouseMove(mouseMotionEvent);
        }

        if (@event is InputEventMouse mouseEvent )
        {
            OnMousePressed(mouseEvent);
        }
    }

    public void OnMousePressed(InputEventMouse mouseEvent)
    {
        Vector3 start = camera.ProjectRayOrigin(mouseEvent.Position);
        Vector3 end = camera.ProjectPosition(mouseEvent.Position, 10);

        PhysicsRayQueryParameters3D parameters = new();
        parameters.From = start;
        parameters.To = end;
        Godot.Collections.Dictionary result = GetWorld3D().DirectSpaceState.IntersectRay(parameters);
        if (result.Count > 0)
        {
            StaticBody3D body = (StaticBody3D)result["collider"];
            if (body.GetChildren().Count < 5)
            {
                selectedSlot = body;
            }
            else
            {
                selectedSlot = null;
            }
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
        if(isBuying)
        {
            foreach (StaticBody3D body in itemSlots)
            {
                Node3D node = (Node3D)body.FindChild("Item_Slot");
                node.Show();
                node = (Node3D)body.FindChild("Item_Slot_Red");
                node.Hide();

                if (selectedSlot != null)
                {
                    node = (Node3D)selectedSlot.FindChild("Item_Slot");
                    node.Hide();
                    node = (Node3D)selectedSlot.FindChild("Item_Slot_Red");
                    node.Show();
                }
            }
        }
        
    }
}
