using Godot;
using System;
using System.Collections.Generic;

public class Room
{
	public String name;
	public String description;
	Room connectingRoomNorth = null;
	Room connectingRoomSouth = null;
	Room connectingRoomWest = null;
	Room connectingRoomEast = null;
	Dictionary<String, Item> Items = new Dictionary<String, Item>();
	
	public Room(String name, String description)
	{
		this.description = description;
		this.name = name;
	}
	
	public String Name
	{
		get { return name; }
	}
	
	public String Description
	{
		get { return description; }
	}
	
	public Room ConnectingRoomNorth
	{
		get { return connectingRoomNorth; }
		set { connectingRoomNorth = value; }
	}
	
	public Room ConnectingRoomSouth
	{
		get { return connectingRoomSouth; }
		set { connectingRoomSouth = value; }
	}
	
	public Room ConnectingRoomEast
	{
		get { return connectingRoomEast; }
		set { connectingRoomEast = value; }
	}
	
	public Room ConnectingRoomWest
	{
		get { return connectingRoomWest; }
		set { connectingRoomWest = value; }
	}
	
	public void AddItem(String itemName, String itemDescription, bool canBePickedUp)
	{
		Items.Add(itemName, new Item(itemName, itemDescription, canBePickedUp));
	}
	
	public Item TakeItem(String itemName)
	{
		Item returnValue = Items[itemName];
		Items.Remove(itemName);
		return returnValue;
	}
	
	public void RemoveItem(String itemName)
	{
		Items.Remove(itemName);
	}
	
	public String GetItemDescription(String itemName)
	{
		return Items[itemName].Description;
	}
	
	public bool IsItemPossibleToTake(String itemName)
	{
		return Items[itemName].CanBePickedUp;
	}
	
	public bool ContainsItem(String itemName)
	{
		return Items.ContainsKey(itemName);
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
				returnValue += "a " + arr[i].Name + ", ";
			}
			
			returnValue += "a " + arr[arr.Count-1].Name + ".";
		}
		
		return returnValue;
	}
}
