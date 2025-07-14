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
	
	public String GetRoomName()
	{
		return _currentRoom.Name;
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
	
	public void AddItemUse(ItemUse itemUse)
	{
		_uses.Add(itemUse);
	}
	
	private ItemUse FindUse(List<ItemType> itemsProvided)
	{
		foreach (ItemUse use in _uses)
		{
			bool itemsFound = true;
			
			foreach (ItemType reqItem in use.RequiredItems)
			{
				if (!itemsProvided.Contains(reqItem))
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
