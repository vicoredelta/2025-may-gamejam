using Godot;
using System;
using System.Collections.Generic;

// Class to represent the entire game world
public class World
{
	// Make singleton, starting room is created here
	private World() { }
	public static World Instance { get; private set; } = new  World();
	
	Dictionary<String, Room> _rooms = new Dictionary<String, Room>();
	Dictionary<String, ItemType> _itemTypes = new Dictionary<String, ItemType>();
	public bool IsPowerOn { get; set; } = false;
	public bool CellIsPlaced { get; set; } = false;
	public int GeneratorPowerLevel { get; set; } = 0;
	
	public Room CreateRoom(String name, String description, String firstTimeDescription = "")
	{
		Room room = new Room(name, description, firstTimeDescription);
		_rooms.Add(name, room);
		return room;
	}
	
	public void SetCurrentRoom(String roomName)
	{
		Player.Instance.CurrentRoom = _rooms[roomName];
		Player.Instance.CurrentRoom.Visited = true;
	}
	
	public void ConnectRooms(String room1Name, String room2Name, Direction direction)
	{
		_rooms[room1Name].Connect(_rooms[room2Name], direction);
	}
	
	public ItemType CreateItemType(String name, String[] aliases, String description, bool canBePickedUp, bool visible = true, String itemPath = "")
	{
		ItemType itemType = new ItemType(name, description, canBePickedUp, visible, itemPath, aliases);
		_itemTypes.Add(name, itemType);
		return itemType;
	}
	
	public ItemType CreateItemType(String name, String[] aliases, String description, bool canBePickedUp, String itemPath)
	{
		ItemType itemType = new ItemType(name, description, canBePickedUp, true, itemPath, aliases);
		_itemTypes.Add(name, itemType);
		return itemType;
	}
	
	public void AddItemToRoom(String itemName, String roomName)
	{
		_rooms[roomName].Add(new Item(_itemTypes[itemName]));
	}
	
	public void AddItemToPlayer(String itemName)
	{
		Player.Instance.Add(new Item(_itemTypes[itemName]));
	}
	
	public void AddItemAsObstacle(String itemName, String room, Direction direction)
	{
		_rooms[room].AddObstacle(new Item(_itemTypes[itemName]), direction);
	}
	
	public ItemType GetItem(String itemName)
	{
		return _itemTypes[itemName];
	}
	
	public UseAction CreateUse(String[] requiredItems, String[] producedItems,
		String[] destroyedItems, ItemCreateLocation createLocation,
		String description, bool requiresPower = false, bool requiresCell = false)
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
		
		UseAction use = new UseAction(description, reqItems, prdItems,
			dstItems, createLocation, requiresPower, requiresCell);
		
		Player.Instance.AddUseAction(use);
			
		return use;
	}
	
	public void CreateInputAction(String requiredItem, List<String> producedItems,
		ItemCreateLocation createLocation, String description, String wrongInputText,
		String requiredText, bool requiresPower = false, bool requiresCell = false)
	{
		List<ItemType> prdItems = new List<ItemType>();
		
		foreach (String itemName in producedItems)
		{
			prdItems.Add(_itemTypes[itemName]);
		}
		
		Player.Instance.AddInputAction(new InputAction(description, requiredText, wrongInputText,
			_itemTypes[requiredItem], prdItems, createLocation, requiresPower, requiresCell));
	}
	
	public Dictionary<string, bool> GetVisitedStatusForAllRooms()
	{
		var visitedStatus = new Dictionary<string, bool>();
		
		foreach (var roomEntry in _rooms)
		{
			string roomName = roomEntry.Key;
			Room room = roomEntry.Value;
			
			visitedStatus[roomName] = room.Visited;
		}
		
		return visitedStatus;
	}
}
