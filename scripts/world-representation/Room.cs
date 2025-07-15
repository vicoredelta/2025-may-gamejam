using Godot;
using System;
using System.Collections.Generic;

public class Room
{
	Inventory _items;
	Room _connectingRoomNorth = null;
	Room _connectingRoomSouth = null;
	Room _connectingRoomEast = null;
	Room _connectingRoomWest = null;
	
	public Room(String name, String description)
	{
		Description = description;
		Name = name;
		_items = new Inventory();
	}
	
	public String Name { get; }
	public String Description { get; }
	
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
		case Direction.North: if (_connectingRoomNorth != null) return _connectingRoomNorth; break;
		case Direction.South: if (_connectingRoomSouth != null) return _connectingRoomSouth; break;
		case Direction.East: if (_connectingRoomEast != null) return _connectingRoomEast; break;
		case Direction.West: if (_connectingRoomWest != null) return _connectingRoomWest; break;
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
	
	public List<TileCoordinate> GenerateTileCoordinates()
	{
		return GenerateTileCoordinates(this, 0, 0, new List<Room>());
	}
	
	private List<TileCoordinate> GenerateTileCoordinates(Room refRoom, float refPosX, float refPosY, List<Room> visitedRooms)
	{
		List<TileCoordinate> list = new List<TileCoordinate>();
		
		if (refRoom != null && !visitedRooms.Contains(refRoom))
		{
			visitedRooms.Add(refRoom);
			list.Add(new TileCoordinate(refRoom.Name, refPosX, refPosY));
			list.AddRange(GenerateTileCoordinates(refRoom._connectingRoomSouth, refPosX, refPosY + 54, visitedRooms));
			list.AddRange(GenerateTileCoordinates(refRoom._connectingRoomNorth, refPosX, refPosY - 54, visitedRooms));
			list.AddRange(GenerateTileCoordinates(refRoom._connectingRoomWest, refPosX - 54, refPosY, visitedRooms));
			list.AddRange(GenerateTileCoordinates(refRoom._connectingRoomEast, refPosX + 54, refPosY, visitedRooms));
		}
		
		return list;
	}
}

public struct TileCoordinate
{
	public TileCoordinate(String name, float x, float y)
	{
		Name = name;
		Position = new Vector2(x, y);
	}
	
	public String Name { get; }
	public Vector2 Position { get; }
}
