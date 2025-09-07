using Godot;
using System;
using System.Collections.Generic;

public class Room : ItemHolder
{
	Dictionary<Direction, Connection> _connections = new Dictionary<Direction, Connection>();
	
	public Room(String name, String description, String firstTimeDescription)
	{
		Description = description;
		Name = name;
		FirstTimeDescription = firstTimeDescription;
	}
	
	public String Name { get; }
	public String Description { get; set; }
	public bool Visited { get; set; } = false;
	public String FirstTimeDescription { get; }
	
	public void Connect(Room destination, Direction direction)
	{
		Connection connection = new Connection(this, destination, direction);
		this._connections.Add(direction, connection);
		destination._connections.Add(Reverse(direction), connection);		
	}
	
	public void AddObstacle(Item item, Direction direction)
	{
		if (_connections.ContainsKey(direction)) _connections[direction].Add(item);
	}
	
	public Room GetConnectingRoom(Direction direction) 
	{
		if (_connections.ContainsKey(direction))
		{
			return _connections[direction].GetDestination(this);
		}
		else
		{
			return null;
		}
	}
	
	public new Item Take(ItemType itemType)
	{
		if (base.HasItem(itemType)) return base.Take(itemType);
		
		foreach (Direction dir in Enum.GetValues(typeof(Direction)))
		{
			if (_connections.ContainsKey(dir) && _connections[dir].HasItem(itemType))
			{
				return _connections[dir].Take(itemType);
			}
		}
		
		return null;
	}
	
	public new bool HasItem(ItemType itemType)
	{
		if (base.HasItem(itemType) == true) return true;
		
		foreach (Direction dir in Enum.GetValues(typeof(Direction)))
		{
			if (_connections.ContainsKey(dir) && _connections[dir].HasItem(itemType))
			{
				return true;
			}
		}
		
		return false;
	}
	
	public new List<ItemType> GetItemTypes()
	{
		List<ItemType> list = new List<ItemType>();
		list.AddRange(base.GetItemTypes());
		
		foreach (var conn in _connections)
		{
			list.AddRange(conn.Value.GetItemTypes());
		}
		
		return list;
	}
	
	public bool ObstaclesExist(Direction direction)
	{
		return (_connections.ContainsKey(direction) && _connections[direction].Count > 0);
	}
	
	public String GetRoomAndItemDescription()
	{
		String text = Description;
			
		if (ListItems() != "")
		{
			text = text + "\n" + ListItems();
		}
	
		return text;
	}
	
	public new String ListItems()
	{
		String text = "";
		
		if (base.ListItems() != "") 
		{
			text = text + base.ListItems() + ".";
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
		if (_connections.ContainsKey(direction) && _connections[direction].ListItems() != "")
		{
			return _connections[direction].ListItems() + " blocking the way " + direction.ToString().ToLower() + ".";
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
			list.AddRange(GenerateTileCoordinates(refRoom.GetConnectingRoom(Direction.South), refPosX, refPosY + 54, visitedRooms));
			list.AddRange(GenerateTileCoordinates(refRoom.GetConnectingRoom(Direction.North), refPosX, refPosY - 54, visitedRooms));
			list.AddRange(GenerateTileCoordinates(refRoom.GetConnectingRoom(Direction.West), refPosX - 54, refPosY, visitedRooms));
			list.AddRange(GenerateTileCoordinates(refRoom.GetConnectingRoom(Direction.East), refPosX + 54, refPosY, visitedRooms));
		}
		
		return list;
	}
	
	private Direction Reverse(Direction direction) => direction switch
	{
		Direction.North => Direction.South,
		Direction.South => Direction.North,
		Direction.East => Direction.West,
		Direction.West => Direction.East,
		_ => Direction.InvalidDirection
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
