using Godot;
using System;
using System.Collections.Generic;

public class ItemType
{
	public ItemType(String name, String description, bool canBePickedUp, bool visible)
	{
		Name = name;
		Description = description;
		CanBePickedUp = canBePickedUp;
		Visible = visible;
	}
	
	public String Name { get; private set; }
	public String Description { get; private set; }
	public bool CanBePickedUp { get; private set; }
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
