using Godot;
using System;

public partial class Item : Node
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
