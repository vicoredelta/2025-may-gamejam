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
		
	}
	
	public void AddItemToPlayer(String itemName, String itemDescription)
	{
		
	}
	
	// Returns true upon a successful move
	public bool Move(String direction)
	{
		return true;
	}
	
	public void AddConnection(String roomName1, String roomName2, Direction direction)
	{
		
	}
}
