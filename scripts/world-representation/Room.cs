using Godot;
using System;
using System.Collections.Generic;

public class Room
{
	Inventory _items = new Inventory();
	Inventory _obstaclesNorth = null;
	Inventory _obstaclesSouth = null;
	Inventory _obstaclesEast = null;
	Inventory _obstaclesWest = null;
	Room _connectingRoomNorth = null;
	Room _connectingRoomSouth = null;
	Room _connectingRoomEast = null;
	Room _connectingRoomWest = null;
	
	public Room(String name, String description)
	{
		Description = description;
		Name = name;
	}
	
	public String Name { get; }
	public String Description { get; }
	
	public void Connect(Room destinationRoom, Direction direction)
	{
		Inventory obstacles = new Inventory();
		
		switch (direction)
		{
			case Direction.North:
				_connectingRoomNorth = destinationRoom;
				destinationRoom._connectingRoomSouth = this;
				_obstaclesNorth = obstacles;
				destinationRoom._obstaclesSouth = obstacles;
				break;
			case Direction.South:
				_connectingRoomSouth = destinationRoom;
				destinationRoom._connectingRoomNorth = this;
				_obstaclesSouth = obstacles;
				destinationRoom._obstaclesNorth = obstacles;
				break;
			case Direction.East:
				_connectingRoomEast = destinationRoom;
				destinationRoom._connectingRoomWest = this;
				_obstaclesEast = obstacles;
				destinationRoom._obstaclesWest = obstacles;
				break;
			case Direction.West:
				_connectingRoomWest = destinationRoom;
				destinationRoom._connectingRoomEast = this;
				_obstaclesWest = obstacles;
				destinationRoom._obstaclesEast = obstacles;
				break;
		}
	}
	
	public void AddItem(Item item)
	{
		_items.Add(item);
	}
	
	public void AddObstacle(Item item, Direction direction)
	{
		GetObstacles(direction).Add(item);
	}
	
	public Room GetConnectingRoom(Direction direction) => direction switch 
	{
		Direction.North => _connectingRoomNorth,
		Direction.South => _connectingRoomSouth,
		Direction.East => _connectingRoomEast,
		Direction.West => _connectingRoomWest,
		_ => null
	};
	
	public Item TakeItem(ItemType itemType)
	{
		if (_items.HasItem(itemType)) return _items.Take(itemType);
		if (_obstaclesNorth != null && _obstaclesNorth.HasItem(itemType)) return _obstaclesNorth.Take(itemType);
		if (_obstaclesSouth != null && _obstaclesSouth.HasItem(itemType)) return _obstaclesSouth.Take(itemType);
		if (_obstaclesEast != null && _obstaclesNorth.HasItem(itemType)) return _obstaclesEast.Take(itemType);
		if (_obstaclesWest != null && _obstaclesNorth.HasItem(itemType)) return _obstaclesWest.Take(itemType);
		return null;
	}
	
	public bool HasItem(ItemType itemType)
	{
		return (_items.HasItem(itemType)
			|| (_obstaclesNorth != null && _obstaclesNorth.HasItem(itemType))
			|| (_obstaclesSouth != null && _obstaclesSouth.HasItem(itemType))
			|| (_obstaclesEast != null && _obstaclesEast.HasItem(itemType))
			|| (_obstaclesWest != null && _obstaclesWest.HasItem(itemType)));
	}
	
	public bool ObstaclesExist(Direction direction)
	{
		Inventory obstacles = GetObstacles(direction);
		
		if (obstacles != null && obstacles.Count > 0)
		{
			return true;
		}
		
		return false;
	}
	
	public String ListItems()
	{
		String text = "";
		
		if (_items.ListItems() != "") 
		{
			text = text + _items.ListItems() + ".";
		}
		
		foreach (var dir in Enum.GetValues<Direction>())
		{
			if (ListObstacles(dir) != "")
			{
				text = text + ListObstacles(dir);
			}
		}
		
		return text;
	}
	
	public String ListObstacles(Direction direction)
	{
		if (GetConnectingRoom(direction) != null && GetObstacles(direction).ListItems() != "")
		{
			return GetObstacles(direction).ListItems() + " blocking the way " + direction.ToString().ToLower() + ".";
		}
		else
		{
			return "";
		}
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
	
	private Inventory GetObstacles(Direction direction) => direction switch 
	{
		Direction.North => _obstaclesNorth,
		Direction.South => _obstaclesSouth,
		Direction.East => _obstaclesEast,
		Direction.West => _obstaclesWest,
		_ => null
	};
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
