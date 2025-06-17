using Godot;
using System;
using System.Collections.Generic;

// Class to represent the entire game world
public class World
{
	Dictionary<String, Room> _rooms = new Dictionary<String, Room>();
	Dictionary<String, ItemType> _itemTypes = new Dictionary<String, ItemType>();
	Player _player;
	
	public World(String startingRoomName, String startingRoomDescription)
	{
		Room startingRoom = new Room(startingRoomName, startingRoomDescription);
		_rooms.Add(startingRoomName, startingRoom);
		_player = new Player(startingRoom);
	}
	
	public void CreateRoom(String name, String description)
	{
		_rooms.Add(name, new Room(name, description));
	}
	
	public void CreateItemType(String name, String description, bool canBePickedUp)
	{
		_itemTypes.Add(name, new ItemType(name, description, canBePickedUp));
	}
	
	public void CreateItemUse(String description, String[] requiredItems,
		String[] producedItems, String[] destroyedItems,
		ItemCreateLocation createLocation)
	{
		List<ItemType> reqItems = new List<ItemType>();
		List<ItemType> prdItems = new List<ItemType>();
		List<ItemType> dstItems = new List<ItemType>();
		
		foreach (String itemName in requiredItems)
		{
			reqItems.Add(_itemTypes[itemName]);
		}
		
		foreach (String itemName in producedItems)
		{
			prdItems.Add(_itemTypes[itemName]);
		}
		
		foreach (String itemName in destroyedItems)
		{
			dstItems.Add(_itemTypes[itemName]);
		}
		
		_player.AddItemUse(new ItemUse(description, reqItems, prdItems,
			dstItems, createLocation));
	}
}
