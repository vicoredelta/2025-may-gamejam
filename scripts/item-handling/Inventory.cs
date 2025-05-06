using Godot;
using System;
using System.Collections.Generic;

public class Inventory
{
	List<Item> items = new List<Item>();
	
	public bool HasItem(ItemType itemType)
	{
		return items.Exists(x => x.Type == itemType);
	}
	
	public Item Take(ItemType itemType)
	{
		if (HasItem(itemType))
		{
			Item returnValue = items.Find(x => x.Type == itemType);
			items.Remove(returnValue);
			return returnValue;
		}
		else
		{
			return null;
		}
	}
	
	public void Add(Item item)
	{
		items.Add(item);
	}
}
