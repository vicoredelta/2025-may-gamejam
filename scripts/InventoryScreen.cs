using Godot;
using System;
using System.Collections.Generic;

public partial class InventoryScreen : ItemList
{
	// List to keep track of which order the items are listed in
	// since Godot doesn't provide a method to access item by name
	List<String> itemList = new List<String>();
	
	public void ModifyInventory(Item item, bool itemIsBeingAdded)
	{
		if (itemIsBeingAdded)
		{
			AddItem(item.Name, null, false);
			itemList.Add(item.Name);
		}
		else
		{
			int index = itemList.IndexOf(item.Name);
			itemList.RemoveAt(index);
			RemoveItem(index);
		}
	}
}
