using Godot;
using System;

public partial class Map : Node
{
	private Node2D RoomHolder;
	
	public void MapMove(String direction, String destinationRoom)
	{
		GD.Print(direction);
		setRoom(direction);
	}
	
	public override void _Ready()
	{
		RoomHolder = GetNode<Node2D>("RoomsHolder");
	}
	
	
	public void setRoom (string currentRoom)
	{
		GD.Print("SetRoom");
		string direction = currentRoom;
		switch (direction)
		{
			
			case "west":
				RoomHolder.Position =  new Vector2(0, 0);
				GD.Print(RoomHolder.Position);
			break;	
			case "east":
				RoomHolder.Position =  new Vector2(0, 54);
				GD.Print(RoomHolder.Position);
			break;	
			case "north":
				RoomHolder.Position =  new Vector2(54, 54);
				GD.Print(RoomHolder.Position);
			break;	
			case "south":
				RoomHolder.Position =  new Vector2(54, 0);
				GD.Print(RoomHolder.Position);
			break;	
		
		}
	}
	
	private void MoveMap ()
	{
		
	}
}
