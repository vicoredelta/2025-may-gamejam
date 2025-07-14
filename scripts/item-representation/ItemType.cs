using Godot;
using System;
using System.Collections.Generic;

public class ItemType
{
	public ItemType(String name, String description, bool canBePickedUp)
	{
		Name = name;
		Description = description;
		CanBePickedUp = canBePickedUp;
	}
	
	public String Name { get; private set; }
	public String Description { get; private set; }
	public bool CanBePickedUp { get; private set; }
}

public class Item
{
	public Item(ItemType itemType)
	{
		Type = itemType;
	}
	
	public ItemType Type { get; private set; }
}
