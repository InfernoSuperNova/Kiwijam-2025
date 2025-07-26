using Godot;
using Kiwijam2025.Debug;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Kiwijam2025.Scripts.Items;

public partial class Game : Node3D
{
    public List<StaticBody3D> itemSlots = [];
    Camera3D camera;
    StaticBody3D selectedSlot;
    RichTextLabel activationText;
    long activations = 100;
    RichTextLabel pointsText;
    long points = 10;
    PackedScene boughtItem = null;
    Dictionary<string, PackedScene> fileToPackedScene = new Dictionary<string, PackedScene>();
    

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
        string[] itemFiles = DirAccess.Open("res://Scenes/Items").GetFiles();
        foreach (string file in itemFiles)
        {
            string name = file.Split(".")[0];
            GD.Print(name);
            fileToPackedScene[name] = GD.Load<PackedScene>("res://Scenes/Items/" + file);
        }
        GD.Print(fileToPackedScene["Sword"]);
    }

    public void BuyItem(string itemName)
    {
        boughtItem = fileToPackedScene[itemName];
        Control shop = GetNode<Control>("Control/Shop");
        shop.Hide();
        Button backButton = GetNode<Button>("Control/ColorRect/BackButton");
        backButton.Hide();
    }

    public void ActivateButton()
    {
        if(activations == 0)
        {
            return;
        }
        activations--;
        activationText.Text = "Activations Left: " + activations;

        foreach(StaticBody3D itemSlot in itemSlots) {
            if(itemSlot.GetChildren().Count == 5 && itemSlot.GetChild(4) is Item item)
            {
                points += item._GeneratePoints();
            }
        }
        
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
        if(!mouseEvent.IsPressed())
        {
            return;
        }
        Vector3 start = camera.ProjectRayOrigin(mouseEvent.Position);
        Vector3 end = camera.ProjectPosition(mouseEvent.Position, 10);

        PhysicsRayQueryParameters3D parameters = new();
        parameters.From = start;
        parameters.To = end;
        Godot.Collections.Dictionary result = GetWorld3D().DirectSpaceState.IntersectRay(parameters);
        if (result.Count > 0)
        {
            StaticBody3D body = (StaticBody3D)result["collider"];
            if (body.GetChildren().Count < 5 && boughtItem != null)
            {
                Node3D instatiatedItem = boughtItem.Instantiate<Node3D>();
                selectedSlot.AddChild(instatiatedItem);
                GD.Print(selectedSlot.GetChildren());
                boughtItem = null;
                Control shop = GetNode<Control>("Control/Shop");
                shop.Show();
                Button backButton = GetNode<Button>("Control/ColorRect/BackButton");
                backButton.Show();
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
            if(body.GetChildren().Count < 5 && boughtItem != null)
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
        foreach (StaticBody3D body in itemSlots)
        {
            Node3D node = (Node3D)body.FindChild("Item_Slot");
            node.Show();
            node = (Node3D)body.FindChild("Item_Slot_Red");
            node.Hide();

            if (boughtItem != null && selectedSlot != null)
            {
                node = (Node3D)selectedSlot.FindChild("Item_Slot");
                node.Hide();
                node = (Node3D)selectedSlot.FindChild("Item_Slot_Red");
                node.Show();
            }
            if (selectedSlot != null)
            {
                node = (Node3D)selectedSlot.FindChild("Item_Slot_Red");
                node.Show();
            }
        }
        
    }
}
