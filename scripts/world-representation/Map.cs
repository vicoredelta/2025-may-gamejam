using Godot;
using System;

public partial class Map : Node
{
	private Node2D RoomHolder;
	private RichTextLabel CurrentRoomText;
	
	public void MapMove(String destinationRoom)
	{
		setRoom(destinationRoom);
	}
	
	public override void _Ready()
	{
		RoomHolder = GetNode<Node2D>("Boundry/Holder/RoomsHolder");
		CurrentRoomText = GetNode<RichTextLabel>("CurrentRoomText");
	}
	
	public void TileCoordinateReceived(String name, Vector2 position)
	{
		var scene = GD.Load<PackedScene>("res://scenes/room.tscn");
		Sprite2D instance = scene.Instantiate<Sprite2D>();
		RoomHolder.AddChild(instance);
		instance.Position = position;
		instance.Name = name;
	}
	
	public void setRoom (String currentRoom)
	{
		CurrentRoomText.Text = currentRoom;	
		Sprite2D node = GetNode<Sprite2D>("Boundry/Holder/RoomsHolder/" + currentRoom);
		RoomHolder.Position = InvertPosition(node.Position);
	}
	
	private Vector2 InvertPosition(Vector2 position)
	{
		return new Vector2(-position.X, -position.Y);
	}
	
}
