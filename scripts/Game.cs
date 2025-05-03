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
		world.AddRoom("Entrance", "This room is very blue");
		world.AddRoom("Hallway", "This room is super red");
		world.AddRoom("Salon", "This room is super red");
		
		// Set starting room
		world.SetCurrentRoom("Entrance");
		
		// Define connections between rooms
		world.AddConnection("Entrance", "Hallway", Direction.North);
		world.AddConnection("Hallway", "Salon", Direction.North);
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
						EmitSignal(SignalName.MapMove, direction, world.GetCurrentRoomName());
					}
					else
					{
						OutputText("There is no where to go " + direction);
					}
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
