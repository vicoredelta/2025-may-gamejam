using Godot;
using System;

public class Item
{
	ItemType itemType;
	
	public Item(ItemType itemType)
	{
		this.itemType = itemType;
	}
	
	public ItemType Type
	{
		get { return itemType; }
	}
}
