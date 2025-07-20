using Godot;
using System;
using System.Collections.Generic;

public class ItemType
{
	public ItemType(String name, String description, bool canBePickedUp, bool visible, String iconPath)
	{
		Name = name;
		Description = description;
		CanBePickedUp = canBePickedUp;
		Visible = visible;
		IconPath = iconPath;
	}
	
	public String IconPath { get; }
	public String Name { get; }
	public String Description { get; }
	public bool CanBePickedUp { get; }
	public bool Visible { get; }
}

public class Item
{
	public Item(ItemType itemType)
	{
		Type = itemType;
	}
	
	public static Item GetItem(ItemType itemType, Inventory inventory1, Inventory inventory2)
	{
		Item item = inventory1.GetItem(itemType);
		
		if (item != null)
		{
			return item;
		}
		else
		{
			return inventory2.GetItem(itemType);
		}
	}
	
	public ItemType Type { get; private set; }
}
