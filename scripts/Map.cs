using Godot;
using System;

public partial class Map : Node
{
	private Node2D RoomHolder;
	private Sprite2D Entrance;
	private Sprite2D Hallway;
	private Sprite2D Salon;
	
	
	public void MapMove(String direction, String destinationRoom)
	{
		setRoom(destinationRoom);
	}
	
	public override void _Ready()
	{
		RoomHolder = GetNode<Node2D>("RoomsHolder");
		Entrance = GetNode<Sprite2D>("RoomsHolder/Entrance");
		Hallway = GetNode<Sprite2D>("RoomsHolder/Entrance");
		Salon = GetNode<Sprite2D>("RoomsHolder/Entrance");
	}
	
	
	public void setRoom (string currentRoom)
	{
		
		switch (currentRoom)
		{
			
			case "Entrance":
				RoomHolder.Position = RoomHolder.Position+Entrance.Position*(-1);
				GD.Print(Entrance.Position);
			break;	
			case "Hallway":
				RoomHolder.Position = RoomHolder.Position+Hallway.Position*(-1);
				GD.Print(RoomHolder.Position);
			break;	
			case "Salon":
				RoomHolder.Position = Salon.Position*(-1);
				GD.Print(RoomHolder.Position);
			break;	
			case "south":
				RoomHolder.Position = new Vector2(54, 0);
				GD.Print(RoomHolder.Position);
			break;	
		
		}
	}
	
	private void MoveMap ()
	{
		
	}
}
