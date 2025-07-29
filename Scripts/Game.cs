using Godot;
using System.Collections.Generic;
using Kiwijam2025.Scripts.Items;
using Kiwijam2025;

public partial class Game : Node3D
{
    public List<StaticBody3D> itemSlots = [];
    Camera3D camera;
    Node3D selectedSlot;
    RichTextLabel activationText;
    long activations = 100;
    RichTextLabel pointsText;
    PackedScene boughtItem = null;
    Dictionary<string, PackedScene> fileToPackedScene = new Dictionary<string, PackedScene>();
    Dictionary<string, string> shopItemToPath = [];
    

    public override void _Ready()
    {
        shopItemToPath.Add("Sword", "Control/Shop/ColorRect/VBoxContainer/HBoxContainer/");
        shopItemToPath.Add("Pan", "Control/Shop/ColorRect/VBoxContainer/HBoxContainer/");
        shopItemToPath.Add("BoxingGlove", "Control/Shop/ColorRect/VBoxContainer/HBoxContainer/");
        shopItemToPath.Add("Bank", "Control/Shop/ColorRect/VBoxContainer/HBoxContainer/");
        shopItemToPath.Add("Guillotine", "Control/Shop/ColorRect/VBoxContainer/HBoxContainer/");

        shopItemToPath.Add("Book", "Control/Shop/ColorRect/VBoxContainer/HBoxContainer2/");
        shopItemToPath.Add("Dam!", "Control/Shop/ColorRect/VBoxContainer/HBoxContainer2/");
        shopItemToPath.Add("Doughnuts", "Control/Shop/ColorRect/VBoxContainer/HBoxContainer2/");
        shopItemToPath.Add("Fries", "Control/Shop/ColorRect/VBoxContainer/HBoxContainer2/");
        shopItemToPath.Add("Graduation had", "Control/Shop/ColorRect/VBoxContainer/HBoxContainer2/");

        shopItemToPath.Add("Ladybeetle", "Control/Shop/ColorRect/VBoxContainer/HBoxContainer3/");
        shopItemToPath.Add("Medal", "Control/Shop/ColorRect/VBoxContainer/HBoxContainer3/");
        shopItemToPath.Add("PileOfCoins", "Control/Shop/ColorRect/VBoxContainer/HBoxContainer3/");
        shopItemToPath.Add("Potion", "Control/Shop/ColorRect/VBoxContainer/HBoxContainer3/");
        shopItemToPath.Add("TV", "Control/Shop/ColorRect/VBoxContainer/HBoxContainer3/");
        base._Ready();
        Node3D activeItemSlots = GetNode<Node3D>("ItemSlots/Active");
        activationText = GetNode<RichTextLabel>("Control/ColorRect/Activations");
        activationText.Text = "Activations Left: " + activations;
        pointsText = GetNode<RichTextLabel>("Control/ColorRect/Points");
        pointsText.Text = "Points: " + PlayerWallet.Points;
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
            string[] splitFile = file.Split(".");
            splitFile = [splitFile[0], splitFile[1]];
            string name = splitFile[0];
            fileToPackedScene[name] = GD.Load<PackedScene>("res://Scenes/Items/" + splitFile.Join("."));
        }
    }

    public void BuyItem(string itemName)
    {
        ShopItem shopItem = GetNode<ShopItem>(shopItemToPath[itemName] + itemName);
        if (shopItem.cost > PlayerWallet.Points)
        {
            return;
        }
        PlayerWallet.Points -= shopItem.cost;
        
        boughtItem = fileToPackedScene[itemName];
        Control shop = GetNode<Control>("Control/Shop");
        
        pointsText.Text = "Points: " + PlayerWallet.Points;
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

        foreach (Item item in Item.All)
        {
            item.original = item.PointGen;
            GD.Print("Item original ", item.original);
        }
        foreach (Item item in Item.All)
        {
            item.Effect.Apply(item);
        }
        foreach (Item item in Item.All) {
            PlayerWallet.Points += (long)item.PointGen;
        }
        foreach (Item item in Item.All)
        {
            item.PointGen = item.original;
            GD.Print("Item original ", item.PointGen);
        }

        pointsText.Text = "Points: " + PlayerWallet.Points;
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
                Item instatiatedItem = boughtItem.Instantiate<Item>();
                string[] split_item_slot = selectedSlot.Name.ToString().Split("_");
                instatiatedItem.GridPosition = new Vector2I(int.Parse(split_item_slot[3]), int.Parse(split_item_slot[2]));
                selectedSlot.AddChild(instatiatedItem);
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
            Node3D body = (Node3D)result["collider"];
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
