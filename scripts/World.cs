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
	
	public String GetCurrentRoomName()
	{
		return currentRoom.Name;
	}
	
	public String Look()
	{
		return currentRoom.Description + currentRoom.ListItems();
	}
	
	public String Examine(String itemName)
	{
		return currentRoom.GetItemDescription(itemName);
	}
	
	public void AddItem(String itemName, String itemDescription, String roomName, bool canBePickedUp)
	{
		if (!rooms.ContainsKey(roomName))
			throw new InvalidOperationException("Room " + roomName + "does not exist");
			
		rooms[roomName].AddItem(itemName, itemDescription, canBePickedUp);
	}
	
	// Returns true upon a successful move
	public bool Move(String direction)
	{
		bool returnValue = true;
		
		switch (direction)
		{
			case "north":
				if (currentRoom.ConnectingRoomNorth != null)
					currentRoom = currentRoom.ConnectingRoomNorth;
				else
					return false;
				break;
			case "south":
				if (currentRoom.ConnectingRoomSouth != null)
					currentRoom = currentRoom.ConnectingRoomSouth;
				else
					return false;
				break;
			case "east":
				if (currentRoom.ConnectingRoomEast != null)
					currentRoom = currentRoom.ConnectingRoomEast;
				else
					return false;
				break;
			case "west":
				if (currentRoom.ConnectingRoomWest != null)
					currentRoom = currentRoom.ConnectingRoomWest;
				else
					return false;
				break;
			default:
				throw new InvalidOperationException(direction + "is not a direction");
				break;
		}
		
		return returnValue;
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
	Dictionary<String, Item> Items = new Dictionary<String, Item>();
	
	public Room(String name, String description)
	{
		Description = description;
		Name = name;
	}
	
	public void AddItem(String itemName, String itemDescription, bool canBePickedUp)
	{
		Items.Add(itemName, new Item(itemName, itemDescription, canBePickedUp));
	}
	
	public String GetItemDescription(String itemName)
	{
		if (!Items.ContainsKey(itemName))
		{
			return "There is no " + itemName + " in inventory or in the vicinity.";
		}
		else
		{
			return Items[itemName].Description;
		}
	}
	
	public String ListItems()
	{
		String returnValue = "";
		
		if (Items.Count > 0)
		{
			returnValue += "\nThere is ";
			List<Item> arr = new List<Item>(Items.Values);
			
			for (int i=0; i<arr.Count-1; i++)
			{
				returnValue += "a " + arr[i].Description + ", ";
			}
			
			returnValue += "a " + arr[0].Name + ".";
		}
		
		return returnValue;
	}
}
