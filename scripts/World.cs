using Godot;
using System;
using System.Collections.Generic;

// Class to represent the entire game world
public class World
{	
	Dictionary<String, Room> rooms = new Dictionary<String, Room>();
	Dictionary<String, Item> inventory = new Dictionary<String, Item>();
	Room currentRoom;
	
	public static bool IsDirection(String text)
	{
		if (text == "north" || text == "south" || text == "east" || text == "west")
			return true;
		else
			return false;
	}
	
	public (String, Item) Take(String itemName)
	{
		if (inventory.ContainsKey(itemName))
		{
			return (itemName + " is already in your inventory.", null);
		}
		else if (!currentRoom.ContainsItem(itemName))
		{
			return ("There is no '" + itemName + "' in the vicinity.", null); 
		}
		else if (!currentRoom.IsItemPossibleToTake(itemName))
		{
			return ("You can not pick up " + itemName + ".", null);
		}
		else
		{
			Item pickup = currentRoom.TakeItem(itemName);
			inventory.Add(pickup.Name, pickup);
			return ("You pick up " + itemName + ".", pickup);
		}
	}
	
	public void AddRoom(String name, String description)
	{
		rooms.Add(name, new Room(name, description));
	}
	
	public String Use(String itemName1, String itemName2)
	{
		String outputText = "Nothing interesting happens.";
		
		if (!(currentRoom.ContainsItem(itemName1) || inventory.ContainsKey(itemName1)))
		{
			return "There is no '" + itemName1 + "' in inventory or vicinity.";
		}
		
		switch (itemName1)
		{
			case "rubble":
				currentRoom.RemoveItem("rubble");
				currentRoom.AddItem("storage", "It has a simple electronic lock.", false);
				outputText = "With little effort the rubble is cleared, revealing a storage box with a simple electronic lock.";
				break;
				
			case "MagiWrench":
				if (itemName2 != null && itemName2 == "storage" && currentRoom.ContainsItem("storage"))
				{
					currentRoom.RemoveItem("storage");
					currentRoom.AddItem("RedCable", "it's a red cable.", true);
					currentRoom.AddItem("GreenCable", "it's a green cable.", true);
					currentRoom.AddItem("BlueCable", "it's a blue cable.", true);
					currentRoom.AddItem("PurpleCable", "it's a purple cable.", true);
					outputText = "With a click and a chime the lock is undone and the box lid opens to reveal a large assortment of coloured cables. The box contains Green, Purple, Red, and Blue cables.";
				}
				break;
				
			default:
				break;
		}
		
		return outputText;
	}
	
	public void SetCurrentRoom(String roomName)
	{
		if (rooms.ContainsKey(roomName))
		{
			currentRoom = rooms[roomName];
		}
		else
		{
			GD.Print("ERROUNOUS CALL OF SetCurrentRoom()!!!");
			throw new InvalidOperationException("Room " + roomName + " does not exist");
		}
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
		if (currentRoom.ContainsItem(itemName))
		{
			return currentRoom.GetItemDescription(itemName);
		}
		else if (inventory.ContainsKey(itemName))
		{
			return inventory[itemName].Description;
		}
		else
		{
			return "There is no '" + itemName + "' in inventory or vicinity.";
		}
	}
	
	public void AddItem(String itemName, String itemDescription, String roomName, bool canBePickedUp)
	{
		if (!rooms.ContainsKey(roomName))
		{
			GD.Print("ERROUNOUS CALL OF AddItem()!!!");
			throw new InvalidOperationException("Room " + roomName + "does not exist");
		}
			
		rooms[roomName].AddItem(itemName, itemDescription, canBePickedUp);
	}
	
	public void AddItemToPlayer(String itemName, String itemDescription)
	{
		if (inventory.ContainsKey(itemName))
		{
			GD.Print("ERROUNOUS CALL OF AttItemToPlayer()!!!");
			throw new InvalidOperationException(itemName + "already exists in inventory");
		}
		
		inventory.Add(itemName, new Item(itemName, itemDescription, true));
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
				GD.Print("ERROUNOUS CALL OF Move()!!!");
				throw new InvalidOperationException(direction + "is not a direction");
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
