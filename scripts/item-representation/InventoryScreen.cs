using Godot;
using System;
using System.Collections.Generic;

public partial class InventoryScreen : ItemList
{
	// List to keep track of which order the items are listed in
	// since Godot doesn't provide a method to access item by name
	List<String> itemList = new List<String>();
	
	InventoryScreen()
	{
		// Starting items need to be added here
		AddItemToInventoryScreen("Wracker");
		AddItemToInventoryScreen("Stolen_Power_Cell");
	}
	
	public void ModifyInventory(String itemName, bool itemIsBeingAdded)
	{
		if (itemIsBeingAdded)
		{
			AddItemToInventoryScreen(itemName);
		}
		else
		{
			RemoveItemFromInventoryScreen(itemName);
		}
	}
	
	private void AddItemToInventoryScreen(String itemName)
	{
		AddItem(itemName, null, false);
		itemList.Add(itemName);
	}
	
	private void RemoveItemFromInventoryScreen(String itemName)
	{
		int index = itemList.IndexOf(itemName);
		itemList.RemoveAt(index);
		RemoveItem(index);
	}
}
