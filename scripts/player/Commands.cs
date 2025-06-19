using Godot;
using System;
using System.Collections.Generic;

public partial class Player
{
	public String Use(List<ItemType> requiredItems)
	{
		ItemUse foundUse = FindUse(requiredItems);
		
		if (foundUse != null)
		{
			return foundUse.Use(_inventory, _currentRoom);
		}
		else
		{
			return "Nothing interesting happens.";
		}
	}
	
	public String Look()
	{
		String itemsText = _currentRoom.ListItems();
		
		if (itemsText == "")
		{
			return _currentRoom.Description;
		}
		else
		{
			return _currentRoom.Description + "\n" + itemsText;
		}
	}
	
	public String Examine(ItemType item)
	{
		return item.Description;
	}
	
	public String Move(Direction direction)
	{
		String output = "There is nowhere to go " + direction + ".";
		
		switch (direction)
		{
		case Direction.North:
			if (_currentRoom.ConnectingRoomNorth != null)
			{
				_currentRoom = _currentRoom.ConnectingRoomNorth;
				output = "You move north.";
			}
			break;
			
		case Direction.South:
			if (_currentRoom.ConnectingRoomSouth != null)
			{
				_currentRoom = _currentRoom.ConnectingRoomSouth;
				output = "You move south.";
			}
			break;
			
		case Direction.East:
			if (_currentRoom.ConnectingRoomEast != null)
			{
				_currentRoom = _currentRoom.ConnectingRoomEast;
				output = "You move east.";
			}
			break;
			
		case Direction.West:
			if (_currentRoom.ConnectingRoomWest != null)
			{
				_currentRoom = _currentRoom.ConnectingRoomWest;
				output = "You move west.";
			}
			break;
		}
		
		return output;
	}
	
	public String Help()
	{
		return "Type [look] or [examine] for a description of an item or your " +
			"current surroundings.\n[walk] or [move] must be followed by a " +
			"direction, such as [north] or [left].\n[take] or [grab] must be " +
			"followed by a noun, such as [key] or [gadget].";
	}
	
	public String Take(ItemType itemType)
	{
		if (!_currentRoom.HasItem(itemType))
		{
			return "There is no " + itemType.Name + " to take.";
		}
		else if (!itemType.CanBePickedUp)
		{
			return "You can not pick up " + itemType.Name;
		}
		else
		{
			_currentRoom.TakeItem(itemType);
			return "You pick up " + itemType.Name;
		}
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
