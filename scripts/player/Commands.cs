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
		if (item != null)
			return item.Description;
		else
			return "Nothing interesting happens.";
	}
	
	public String Move(Direction direction)
	{
		String output = "There is nowhere to go " + direction.ToString().ToLower() + ".";
		
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
		case Direction.InvalidDirection:
			output = "Nothing interesting happens.";
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
		if (itemType == null)
		{
			return "Nothing interesting happens.";
		}
		else if (!_currentRoom.HasItem(itemType))
		{
			return "There is no " + itemType.Name.ToLower() + " to take.";
		}
		else if (!itemType.CanBePickedUp)
		{
			return "You can not pick up the " + itemType.Name.ToLower() + ".";
		}
		else
		{
			_currentRoom.TakeItem(itemType);
			return "You pick up the " + itemType.Name.ToLower() + ".";
		}
	}
}
