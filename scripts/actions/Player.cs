using Godot;
using System;

public partial class Player
{
	Room currentRoom;
	//Todo: add inventory
	
	public Player(Room startingRoom, ItemType[] startingItems)
	{
		currentRoom = startingRoom;
		//Todo: add starting items to inventory
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
}
