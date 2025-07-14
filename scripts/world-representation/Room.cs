using Godot;
using System;
using System.Collections.Generic;

public class Room
{
	Inventory _items;
	
	public Room(String name, String description)
	{
		Description = description;
		Name = name;
		_items = new Inventory();
	}
	
	public String Name { get; }
	public String Description { get; }
	public Room ConnectingRoomNorth { get; private set; } = null;
	public Room ConnectingRoomSouth { get; private set; } = null;
	public Room ConnectingRoomEast { get; private set; } = null;
	public Room ConnectingRoomWest { get; private set; } = null;
	
	public void Connect(Room destinationRoom, Direction direction)
	{
		switch (direction)
		{
			case Direction.North:
				ConnectingRoomNorth = destinationRoom;
				destinationRoom.ConnectingRoomSouth = this;
				break;
			case Direction.South:
				ConnectingRoomSouth = destinationRoom;
				destinationRoom.ConnectingRoomNorth = this;
				break;
			case Direction.East:
				ConnectingRoomEast = destinationRoom;
				destinationRoom.ConnectingRoomWest = this;
				break;
			case Direction.West:
				ConnectingRoomWest = destinationRoom;
				destinationRoom.ConnectingRoomEast = this;
				break;
		}
	}
	
	public Room GetConnectingRoom(Direction direction)
	{
		switch (direction)
		{
		case Direction.North: if (ConnectingRoomNorth != null) return ConnectingRoomNorth; break;
		case Direction.South: if (ConnectingRoomSouth != null) return ConnectingRoomSouth; break;
		case Direction.East: if (ConnectingRoomEast != null) return ConnectingRoomEast; break;
		case Direction.West: if (ConnectingRoomWest != null) return ConnectingRoomWest; break;
		case Direction.InvalidDirection:
			break;
		}
		
		return null;
	}
	
	public void AddItem(Item item)
	{
		_items.Add(item);
	}
	
	public Item TakeItem(ItemType itemType)
	{
		return _items.Take(itemType);
	}
	
	public bool HasItem(ItemType itemType)
	{
		return _items.HasItem(itemType);
	}
	
	public String ListItems()
	{
		return _items.ListItems();
	}
}
