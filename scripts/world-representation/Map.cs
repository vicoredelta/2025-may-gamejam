using Godot;
using System;
using System.Collections.Generic; 
using Godot.Collections; 

public partial class Map : Node
{
	private Node2D RoomHolder;
	private RichTextLabel CurrentRoomText;
	
	private System.Collections.Generic.Dictionary<string, Sprite2D> roomSprites = new System.Collections.Generic.Dictionary<string, Sprite2D>();
	
	public void MapMove(String destinationRoom, Godot.Collections.Dictionary<string, bool> visitedStatusForAllRooms)
	{
		setRoom(destinationRoom, visitedStatusForAllRooms);
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
		roomSprites[name] = instance;
	}
	
	public void setRoom (String currentRoom, Godot.Collections.Dictionary<string, bool> visitedStatusForAllRooms)
	{
		CurrentRoomText.Text = currentRoom;	
		Sprite2D node = GetNode<Sprite2D>("Boundry/Holder/RoomsHolder/" + currentRoom);
		RoomHolder.Position = InvertPosition(node.Position);
		SetAllRoomOpacities(currentRoom, visitedStatusForAllRooms);
	}
	
	public void SetAllRoomOpacities(String currentRoom, Godot.Collections.Dictionary<string, bool> visitedStatusForAllRooms)
	{
		foreach (var roomEntry in visitedStatusForAllRooms)
		{
			string roomName = roomEntry.Key;
			bool visited = roomEntry.Value;	
			
			if (roomSprites.TryGetValue(roomName, out Sprite2D sprite))
			{
				float opacity = 0.0f;
				
				if (roomName == currentRoom){
					opacity = 1.0f;
				} else if (visited)
				{
					opacity = 0.5f;
				}
				SetOpacity(sprite, opacity);
			}
		}
	}
	
	public void SetOpacity (CanvasItem node, float alpha)
	{
		if (node == null) {return;}
		
		var color = node.Modulate;
		color.A = Mathf.Clamp(alpha, 0f, 1f);
		node.Modulate = color;
	}
	
	
	private Vector2 InvertPosition(Vector2 position)
	{
		return new Vector2(-position.X, -position.Y);
	}
	
}
