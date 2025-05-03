using Godot;
using System;
using System.Collections.Generic;

// Class to represent the entire game world
public class World
{
	Dictionary<String, Room> rooms = new Dictionary<String, Room>();
	List<(String, String)> connections = new List<(String, String)>();
	
	public void AddRoom(String name, String description)
	{
		rooms.Add(name, new Room(description));
	}
	
	public void AddConnection(String roomName1, String roomName2)
	{
		connections.Add((roomName1, roomName2));
	}
}

public class Room
{
	String Description;
	//List<Item> items;
	
	public Room(String description)
	{
		Description = description;
	}
}
