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
	Room currentRoom;
	
	public static bool IsDirection(String text)
	{
		if (text == "north" || text == "south" || text == "east" || text == "west")
			return true;
		else
			return false;
	}
	
	public void AddRoom(String name, String description)
	{
		rooms.Add(name, new Room(name, description));
	}
	
	public void SetCurrentRoom(String roomName)
	{
		if (rooms.ContainsKey(roomName))
			currentRoom = rooms[roomName];
		else
			throw new InvalidOperationException("Room " + roomName + "does not exist");
	}
	
	public String Look()
	{
		return currentRoom.Description;
	}
	
	public void AddConnection(String roomName1, String roomName2, Direction direction)
	{
		Room room1 = rooms[roomName1];
		Room room2 = rooms[roomName2];
		
		switch (direction)
		{
			case Direction.North:
				room1.ConnectingRoomNorth = room2;
				room2.ConnectingRoomSouth = room1;
				break;
			case Direction.South:
				room1.ConnectingRoomSouth = room2;
				room2.ConnectingRoomNorth = room1;
				break;
			case Direction.East:
				room1.ConnectingRoomEast = room2;
				room2.ConnectingRoomWest = room1;
				break;
			case Direction.West:
				room1.ConnectingRoomWest = room2;
				room2.ConnectingRoomEast = room1;
				break;
		}
	}
}

public class Room
{
	public String Description;
	public String Name;
	public Room ConnectingRoomNorth = null;
	public Room ConnectingRoomSouth = null;
	public Room ConnectingRoomWest = null;
	public Room ConnectingRoomEast = null;
	
	public Room(String name, String description)
	{
		Description = description;
		Name = name;
	}
}
