using Godot;
using System;

public class Player
{
	Room currentRoom;
	Inventory inventory;
	
	public Player(Room startingRoom, Item[] startingItems = null)
	{
		currentRoom = startingRoom;
		inventory = new Inventory();
		
		if (startingItems != null)
		{
			foreach (Item item in startingItems)
			{
				inventory.Add(item);
			}
		}
	}
	
	public String Use(String item1, String item2)
	{
		return "";
	}
	
	public String Look()
	{
		return "";
	}
	
	public String Move(Direction direction)
	{
		return "";
	}
	
	public String Help()
	{
		return "";
	}
	
	public String Take(String item)
	{
		return "";
	}
	
	public void AddItem(Item item)
	{
		inventory.Add(item);
	}
	
	public Item TakeItem(ItemType itemType)
	{
		return inventory.Take(itemType);
	}
	
	public bool HasItem(ItemType itemType)
	{
		return inventory.HasItem(itemType);
	}
}
