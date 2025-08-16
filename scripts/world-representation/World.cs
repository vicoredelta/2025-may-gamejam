using Godot;
using System;
using System.Collections.Generic;

// Class to represent the entire game world
public class World
{
	Dictionary<String, Room> _rooms = new Dictionary<String, Room>();
	Dictionary<String, ItemType> _itemTypes = new Dictionary<String, ItemType>();
	public bool IsPowerOn { get; set; } = false;
	Player _player;
	
	public World(String startingRoomName, String startingRoomDescription)
	{
		Room startingRoom = new Room(startingRoomName, startingRoomDescription, "");
		startingRoom.Visited = true;
		_rooms.Add(startingRoomName, startingRoom);
		_player = new Player(startingRoom, this);
	}
	
	public Room CreateRoom(String name, String description, String firstTimeDescription = "")
	{
		Room room = new Room(name, description, firstTimeDescription);
		_rooms.Add(name, room);
		return room;
	}
	
	public void ConnectRooms(String room1Name, String room2Name, Direction direction)
	{
		_rooms[room1Name].Connect(_rooms[room2Name], direction);
	}
	
	public ItemType CreateItemType(String name, String description, bool canBePickedUp, bool visible = true, String itemPath = "")
	{
		ItemType itemType = new ItemType(name, description, canBePickedUp, visible, "");
		_itemTypes.Add(name, itemType);
		return itemType;
	}
	
	public void CreateItemType(String name, String description, bool canBePickedUp, String itemPath)
	{
		_itemTypes.Add(name, new ItemType(name, description, canBePickedUp, true, itemPath));
	}
	
	public void AddItemToRoom(String itemName, String roomName)
	{
		_rooms[roomName].Add(new Item(_itemTypes[itemName]));
	}
	
	public void AddItemToPlayer(String itemName)
	{
		_player.Add(new Item(_itemTypes[itemName]));
	}
	
	public void AddItemAsObstacle(String itemName, String room, Direction direction)
	{
		_rooms[room].AddObstacle(new Item(_itemTypes[itemName]), direction);
	}
	
	public List<ItemType> GetItemTypes()
	{
		List<ItemType> itemTypeList = new List<ItemType>();
		itemTypeList.AddRange(_itemTypes.Values);
		return itemTypeList;
	}
	
	public CommandResult ExecuteCommand (CommandInput commandInput)
	{
		return _player.ExecuteCommand(commandInput);
	}
	
	public String GetRoomName()
	{
		return _player.GetRoomName();
	}
	
	public ItemType GetItem(String itemName)
	{
		return _itemTypes[itemName];
	}
	
	public ItemUse CreateUse(String[] requiredItems, String[] producedItems,
		String[] destroyedItems, ItemCreateLocation createLocation,
		bool reqPower, String description)
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
		
		ItemUse use = new ItemUse(description, reqItems, prdItems,
			dstItems, createLocation, reqPower);
		
		_player.AddItemUse(use);
			
		return use;
	}
	
	public void CreateInputAction(String requiredItem, List<String> producedItems,
		ItemCreateLocation createLocation, bool reqPower, String description, String wrongInputText,
		String requiredText)
	{
		List<ItemType> prdItems = new List<ItemType>();
		
		foreach (String itemName in producedItems)
		{
			prdItems.Add(_itemTypes[itemName]);
		}
		
		_player.AddInputAction(new InputAction(description, requiredText, wrongInputText,
			_itemTypes[requiredItem], prdItems, createLocation, reqPower));
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
	
	public List<TileCoordinate> GenerateTileCoordinates()
	{
		return 	_rooms[_player.GetRoomName()].GenerateTileCoordinates();
	}
}
