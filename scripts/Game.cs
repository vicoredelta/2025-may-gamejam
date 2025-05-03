using Godot;
using System;
using System.Collections.Generic;

public partial class Game : Node
{
	World world = new World();
	
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
		world.SetCurrentRoom("West room");
		
		// Define connections between rooms
		world.AddConnection("West room", "East room", Direction.East);
	}
	
	private void OutputText(String text)
	{
		EmitSignal(SignalName.TextOutput, text + "\n");
	}
	
	public void TextInputReceived(String textInput)
	{
		// Echo input in output window
		OutputText(">" + textInput);
		
		// Split line into separate words
		string[] words = textInput.Split(' ');
		
		switch (words[0].ToLower())
		{
			case "look":
				OutputText(world.Look());
				break;
				
			case "examine":
				
				break;
				
			case "move":
				if(words.Length < 2)
				{
					OutputText("'Move' requires an argument");
				}
				else if (World.IsDirection(words[1].ToLower()))
				{
					String direction = words[1].ToLower();
					
					if (world.Move(direction))
					{
						OutputText("You move to " + world.GetCurrentRoomName());
					}
					else
					{
						OutputText("There is no where to go " + direction);
					}
					
					EmitSignal(SignalName.MapMove, direction);
				}
				else
				{
					OutputText("'" + words[1] + "'" + " is not a valid direction");
				}
				break;
				
			default:
				OutputText("Invalid command");
				break;
		}
	}
}
