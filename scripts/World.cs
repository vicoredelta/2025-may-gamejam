using Godot;
using System;
using System.Collections.Generic;

public enum Direction
{
	North,
	West,
	East,
	South
}

// Class to represent the entire game world
public class World
{	
	Dictionary<String, Room> rooms = new Dictionary<String, Room>();
	
	public static bool IsDirection(String text)
	{
		if (text == "north" || text == "south" || text == "east" || text == "west")
			return true;
		else
			return false;
	}
	
	public void AddRoom(String name, String description)
	{
		rooms.Add(name, new Room(description));
	}
	
	public String GetRoomDescription(String roomName)
	{
		return rooms[roomName].Description;
	}
	
	public void AddConnection(String roomName1, String roomName2, Direction direction)
	{
		switch (direction)
		{
			case Direction.North:
				rooms[roomName1].ConnectingRoomNorth = roomName2;
				rooms[roomName2].ConnectingRoomSouth = roomName1;
				break;
			case Direction.South:
				rooms[roomName1].ConnectingRoomSouth = roomName2;
				rooms[roomName2].ConnectingRoomNorth = roomName1;
				break;
			case Direction.East:
				rooms[roomName1].ConnectingRoomEast = roomName2;
				rooms[roomName2].ConnectingRoomWest = roomName1;
				break;
			case Direction.West:
				rooms[roomName1].ConnectingRoomWest = roomName2;
				rooms[roomName2].ConnectingRoomEast = roomName1;
				break;
		}
	}
}

public class Room
{
	public String Description;
	public String ConnectingRoomNorth;
	public String ConnectingRoomSouth;
	public String ConnectingRoomWest;
	public String ConnectingRoomEast;
	
	public Room(String description)
	{
		Description = description;
	}
}
