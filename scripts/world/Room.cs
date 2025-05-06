using Godot;
using System;
using System.Collections.Generic;

public class Room
{
	String name;
	String description;
	Room connectingRoomNorth = null;
	Room connectingRoomSouth = null;
	Room connectingRoomWest = null;
	Room connectingRoomEast = null;
	Inventory items;
	
	public Room(String name, String description)
	{
		this.description = description;
		this.name = name;
		items = new Inventory();
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
	}
	
	public Room ConnectingRoomSouth
	{
		get { return connectingRoomSouth; }
	}
	
	public Room ConnectingRoomEast
	{
		get { return connectingRoomEast; }
	}
	
	public Room ConnectingRoomWest
	{
		get { return connectingRoomWest; }
	}
	
	public void Connect(Room destinationRoom, Direction direction)
	{
		switch (direction)
		{
			case Direction.North:
				this.connectingRoomNorth = destinationRoom;
				destinationRoom.connectingRoomSouth = this;
				break;
			case Direction.South:
				this.connectingRoomSouth = destinationRoom;
				destinationRoom.connectingRoomNorth = this;
				break;
			case Direction.East:
				this.connectingRoomEast = destinationRoom;
				destinationRoom.connectingRoomWest = this;
				break;
			case Direction.West:
				this.connectingRoomWest = destinationRoom;
				destinationRoom.connectingRoomEast = this;
				break;
		}
	}
	
	public void AddItem(Item item)
	{
		items.Add(item);
	}
	
	public Item TakeItem(ItemType itemType)
	{
		return items.Take(itemType);
	}
	
	public bool HasItem(ItemType itemType)
	{
		return items.HasItem(itemType);
	}
}
