using Godot;
using System;
using System.Collections.Generic;

public class Room
{
	Inventory _items = new Inventory();
	Dictionary<Direction, Inventory> _obstacles = new Dictionary<Direction, Inventory>();
	Room _connectingRoomNorth = null;
	Room _connectingRoomSouth = null;
	Room _connectingRoomEast = null;
	Room _connectingRoomWest = null;
	
	public Room(String name, String description)
	{
		Description = description;
		Name = name;
		
		foreach (var direction in Enum.GetValues<Direction>())
		{
			if (direction != Direction.InvalidDirection)
				_obstacles.Add(direction, new Inventory());
		}
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
	
	public void AddObstacle(Room connectingRoom, Item item)
	{
		_obstacles[GetDirection(connectingRoom)].Add(item);
	}
	
	public Direction GetDirection(Room connectingRoom)
	{
		if (connectingRoom._connectingRoomNorth == this) return Direction.South;
		if (connectingRoom._connectingRoomSouth == this) return Direction.North;
		if (connectingRoom._connectingRoomWest == this) return Direction.East;
		if (connectingRoom._connectingRoomEast == this) return Direction.West;
		return Direction.InvalidDirection;
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
		String text = _items.ListItems();
		
		if (text != "") 
		{
			text = text + ".";
		}
		
		return text;
	}
	
	public String ListObstacles()
	{
		String text = "";
		
		foreach (var direction in Enum.GetValues<Direction>())
		{
			if (direction != Direction.InvalidDirection &&
				_obstacles[direction].ListItems() != "")
			{
				text = text + "\n" + _obstacles[direction].ListItems() + " blocking the way " + direction.ToString().ToLower() + ".";
			}
		}
		return text;
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
