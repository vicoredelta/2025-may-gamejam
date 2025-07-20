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
	public String Description { get; set; }
	public bool CanBePickedUp { get; }
	public bool Visible { get; }
}

public class Item
{
	public Item(ItemType itemType)
	{
		Type = itemType;
	}
	
	public ItemType Type { get; private set; }
}
