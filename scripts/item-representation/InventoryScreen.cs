using Godot;
using System;
using System.Collections.Generic;

public partial class InventoryScreen : ItemList
{
	// List to keep track of which order the items are listed in
	// since Godot doesn't provide a method to access item by name
	List<String> itemList = new List<String>();
	
	public void ModifyInventory(String itemName, String iconPath, bool itemIsBeingAdded)
	{
		if (itemIsBeingAdded)
		{
			AddItemToInventoryScreen(itemName, iconPath);
		}
		else
		{
			RemoveItemFromInventoryScreen(itemName);
		}
	}
	
	private void AddItemToInventoryScreen(String itemName, String iconPath)
	{
		if (iconPath != "")
		{
			AddItem(itemName, GD.Load<Texture2D>(iconPath), false);
		}
		else
		{
			AddItem(itemName, null, false);
		}
		
		itemList.Add(itemName);
	}
	
	private void RemoveItemFromInventoryScreen(String itemName)
	{
		int index = itemList.IndexOf(itemName);
		itemList.RemoveAt(index);
		RemoveItem(index);
	}
}
