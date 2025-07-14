using Godot;
using System;

public partial class Map : Node
{
	private Node2D RoomHolder;
	private Sprite2D Entrance;
	private Sprite2D Hallway;
	private Sprite2D Salon;
	private RichTextLabel CurrentRoomText;
	
	public void MapMove(String direction, String destinationRoom)
	{
		GD.Print(destinationRoom);
		setRoom(destinationRoom);
	}
	
	public override void _Ready()
	{
		RoomHolder = GetNode<Node2D>("Boundry/Holder/RoomsHolder");
		Entrance = GetNode<Sprite2D>("Boundry/Holder/RoomsHolder/Entrance");
		Hallway = GetNode<Sprite2D>("Boundry/Holder/RoomsHolder/Hallway");
		Salon = GetNode<Sprite2D>("Boundry/Holder/RoomsHolder/Salon");
		CurrentRoomText = GetNode<RichTextLabel>("CurrentRoomText");
	}
	
	public void setRoom (string currentRoom)
	{
		CurrentRoomText.Text = currentRoom;	
		
		switch (currentRoom)
		{
			
			case "Breached Entrance":
				Vector2 EntrancePos = InvertPosition(Entrance.Position);
				RoomHolder.Position = EntrancePos;
				GD.Print(Entrance.Position);
			break;	
			case "Cramped Hallway":
				Vector2 HallwayPos = InvertPosition(Hallway.Position);
				RoomHolder.Position = HallwayPos;
				GD.Print(Hallway.Position);
			break;	
			case "Main Bridge":
				Vector2 SalonPos = InvertPosition(Salon.Position);
				RoomHolder.Position = SalonPos;
				GD.Print(RoomHolder.Position);
			break;	
			case "south":
				RoomHolder.Position = new Vector2(54, 0);
				GD.Print(RoomHolder.Position);
			break;	
		
		}
	}
	private Vector2 InvertPosition(Vector2 position)
	{
		return new Vector2(-position.X, -position.Y);
	}
	
}
