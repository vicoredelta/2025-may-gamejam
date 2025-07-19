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
		startingRoom.Visited = true;
		_rooms.Add(startingRoomName, startingRoom);
		_player = new Player(startingRoom);
	}
	
	public void CreateRoom(String name, String description)
	{
		_rooms.Add(name, new Room(name, description));
	}
	
	public void ConnectRooms(String room1Name, String room2Name, Direction direction)
	{
		_rooms[room1Name].Connect(_rooms[room2Name], direction);
	}
	
	public void CreateItemType(String name, String description, bool canBePickedUp, bool visible = true)
	{
		_itemTypes.Add(name, new ItemType(name, description, canBePickedUp, visible));
	}
	
	public void AddItemToRoom(String itemName, String roomName)
	{
		_rooms[roomName].AddItem(new Item(_itemTypes[itemName]));
	}
	
	public void AddItemToPlayer(String itemName)
	{
		_player.AddItem(new Item(_itemTypes[itemName]));
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
	
	public CommandOutput ExecuteCommand (CommandInput commandInput)
	{
		return _player.ExecuteCommand(commandInput);
	}
	
	public String GetRoomName()
	{
		return _player.GetRoomName();
	}
	
	public void CreateUse(String[] requiredItems, String[] producedItems,
		String[] destroyedItems, ItemCreateLocation createLocation,
		String description)
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
