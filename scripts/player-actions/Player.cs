using Godot;
using System;
using System.Collections.Generic;

public partial class Player : Inventory
{
	Room _currentRoom;
	List<ItemUse> _uses = new List<ItemUse>(); 
	List<InputAction> _inputActions = new List<InputAction>();
	
	public Player(Room startingRoom)
	{
		_currentRoom = startingRoom;
	}
	
	public String GetRoomName()
	{
		return _currentRoom.Name;
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
