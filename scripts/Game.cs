using Godot;
using System;
using System.Collections.Generic;

public partial class Game : Node
{
	World world = new World();
	String currentRoom;
	
	[Signal]
	public delegate void TextOutputEventHandler();
	
	[Signal]
	public delegate void MapMoveEventHandler();
	
	// Constructor, create the entire world here
	Game()
	{
		// Create rooms
		world.AddRoom("West room", "This room is very blue");
		world.AddRoom("East room", "This room is super red");
		
		// Set starting room
		currentRoom = "West room";
		
		// Define connections between rooms
		world.AddConnection("West room", "East room", Direction.East);
	}
	
	public void TextInputReceived(String textInput)
	{
		// Echo input in output window
		EmitSignal(SignalName.TextOutput, "> " + textInput + "\n");
		
		// Split line into separate words
		string[] words = textInput.Split(' ');
		
		switch (words[0].ToLower())
		{
			case "look":
				EmitSignal(SignalName.TextOutput, world.GetRoomDescription(currentRoom) + "\n");
				break;
				
			case "examine":
				
				break;
				
			case "move":
				EmitSignal(SignalName.MapMove, words[1].ToLower());
				break;
			default:
				EmitSignal(SignalName.TextOutput, "Invalid command\n");
				break;
		}
	}
}
