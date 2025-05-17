using Godot;
using System;
using System.Collections.Generic;

public partial class Player
{
	Room _currentRoom;
	Inventory _inventory;
	List<ItemUse> _uses;
	
	public Player(Room startingRoom)
	{
		_currentRoom = startingRoom;
		_inventory = new Inventory();
		_uses = new List<ItemUse>();
	}
	
	public void AddItem(Item item)
	{
		_inventory.Add(item);
	}
	
	public Item TakeItem(ItemType itemType)
	{
		return _inventory.Take(itemType);
	}
	
	public bool HasItem(ItemType itemType)
	{
		return _inventory.HasItem(itemType);
	}
	
	private ItemUse FindUse(List<ItemType> requiredItems)
	{
		foreach (ItemUse use in _uses)
		{
			bool itemsFound = true;
			
			foreach (ItemType reqItem in requiredItems)
			{
				if (!use.RequiredItems.Contains(reqItem))
				{
					itemsFound = false;
					break;
				}
			}
			
			if (itemsFound)
			{
				return use;
			}
		}
		
		return null;
	}
}
