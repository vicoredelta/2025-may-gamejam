using Godot;
using System;
using System.Collections.Generic;

public class Room
{
	String _name;
	String _description;
	Room _connectingRoomNorth = null;
	Room _connectingRoomSouth = null;
	Room _connectingRoomWest = null;
	Room _connectingRoomEast = null;
	Inventory _items;
	
	public Room(String name, String description)
	{
		_description = description;
		_name = name;
		_items = new Inventory();
	}
	
	public String Name
	{
		get { return _name; }
	}
	
	public String Description
	{
		get { return _description; }
	}
	
	public Room ConnectingRoomNorth
	{
		get { return _connectingRoomNorth; }
	}
	
	public Room ConnectingRoomSouth
	{
		get { return _connectingRoomSouth; }
	}
	
	public Room ConnectingRoomEast
	{
		get { return _connectingRoomEast; }
	}
	
	public Room ConnectingRoomWest
	{
		get { return _connectingRoomWest; }
	}
	
	public void Connect(Room destinationRoom, Direction direction)
	{
		switch (direction)
		{
			case Direction.North:
				_connectingRoomNorth = destinationRoom;
				destinationRoom._connectingRoomSouth = this;
				break;
			case Direction.South:
				_connectingRoomSouth = destinationRoom;
				destinationRoom._connectingRoomNorth = this;
				break;
			case Direction.East:
				_connectingRoomEast = destinationRoom;
				destinationRoom._connectingRoomWest = this;
				break;
			case Direction.West:
				_connectingRoomWest = destinationRoom;
				destinationRoom._connectingRoomEast = this;
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
