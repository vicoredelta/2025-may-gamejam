using Godot;
using System;

public partial class Map : Node
{
	private Node2D RoomHolder;
	
	public override void _Ready()
	{
		RoomHolder = GetNode<Node2D>("RoomsHolder");
	}
	
	/*
	public void setRoom (string currentRoom)
	{
		switch (currentRoom)
		{
			/*
			case x:
				RoomHolder.Position =  new Vector2(0, 0);
				
			break;	
			case y:
				RoomHolder.Position =  new Vector2(0, 54);
			break;	
			case z:
				RoomHolder.Position =  new Vector2(54, 54);
			break;	
		
		}
	}
	*/
	private void MoveMap ()
	{
		
	}
}
