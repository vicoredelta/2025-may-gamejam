using Godot;
using System;
using System.Collections.Generic;

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
	
	public Item TakeItem(String itemName)
	{
		Item returnValue = Items[itemName];
		Items.Remove(itemName);
		return returnValue;
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
				returnValue += "a " + arr[i].Description + ", ";
			}
			
			returnValue += "a " + arr[0].Name + ".";
		}
		
		return returnValue;
	}
}
