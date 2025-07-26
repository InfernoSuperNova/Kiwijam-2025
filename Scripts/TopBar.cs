using Godot;
using System;

public partial class TopBar : ColorRect
{
    void ShopButton()
    {
        Control shop = GetNode<Control>("../Shop");
        shop.Show();
        Button shopButton = GetNode<Button>("ShopButton");
        shopButton.Hide();
        Button backButton = GetNode<Button>("BackButton");
        backButton.Show();
    }

    void BackButton()
    {
        Control shop = GetNode<Control>("../Shop");
        shop.Hide();
        Button backButton = GetNode<Button>("BackButton");
        backButton.Hide();
        Button shopButton = GetNode<Button>("ShopButton");
        shopButton.Show();
    }
}
