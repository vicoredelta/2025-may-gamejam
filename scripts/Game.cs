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
	
	[Signal]
	public delegate void ModifyInventoryEventHandler();
	
	// Constructor, instantiate world objects here
	Game()
	{

		// Create rooms (name, descripion)
		world.AddRoom("Entrance",
		"The scant beams of sunlight piercing through the broken hull and gives life to the coffin-like silence. Had the ship crashed elsewhere it might’ve been taken back by nature, but as it stands, the craft remains is somehow even more silent than the wasteland surrounding it. A mechanical cave devoid of life.");
		world.AddRoom("Hallway", "This room is super red");
		world.AddRoom("Salon", "This room is super red");

		// Set starting room
		world.SetCurrentRoom("Entrance");
		
		// Define connections between rooms (room 1, room 2, direction when moving from room 1 to room 2)
		world.AddConnection("Entrance", "Hallway", Direction.North);
		world.AddConnection("Hallway", "Salon", Direction.North);

		// Add items to rooms (item name, item description, room name, can be picked up)
		world.AddItem("rubble",
		"The rubble is sharp but not heavy. It’d be easy to remove with just your hands.",
		"Salon", false);
		
		// Add player starting items here (Need to be added in InventoryScreen.cs as well!)
		world.AddItemToPlayer("MagiWrench",
		"It’s a MagiWrench, a transforming multitool. It’s almost brand new.");
		world.AddItemToPlayer("Generator",
		"It’s a Generator. It has three power regulator knobs. There is a Blue and Green cable attached to two of them. The last one seems to be missing.");
		
	}
	
	private void OutputText(String text)
	{
		EmitSignal(SignalName.TextOutput, text + "\n");
	}
	
	public void PlayerInput(String textInput)
	{
		// Echo input in output window
		OutputText(">" + textInput);
		
		// Split line into separate words
		string[] words = textInput.Split(' ');
		
		switch (words[0].ToLower())
		{
			case "inspect":
			case "examine":
			case "look":
				if (words.Length < 2)
				{
					OutputText(world.Look());
				}
				else if (words[1] == "at" && words.Length > 2)
				{
					OutputText(world.Examine(words[2]));
				}
				else
				{
					OutputText(world.Examine(words[1]));
				}
				break;
				
			case "take":
				if(words.Length < 2)
				{
					OutputText("'take' requires an argument.");
				}
				else
				{
					(String message, Item item) = world.Take(words[1]);
					OutputText(message);
					
					// Add picked up item to inventory screen
					if (item != null)
					{
						EmitSignal(SignalName.ModifyInventory, item, true);
					}
				}
				break;
				
			case "move":
				if(words.Length < 2)
				{
					OutputText("'move' requires an argument.");
				}
				else if (World.IsDirection(words[1].ToLower()))
				{
					String direction = words[1].ToLower();
					bool moveSuccessfull = world.Move(direction);
					
					if (moveSuccessfull)
					{
						OutputText("You move to " + world.GetCurrentRoomName() + ".");
						EmitSignal(SignalName.MapMove, direction, world.GetCurrentRoomName());
						AudioManager.Instance.PlaySFX("walk");
					}
					else
					{
						OutputText("There is no where to go " + direction + ".");
					}
				}
				else
				{
					OutputText("'" + words[1] + "'" + " is not a valid direction.");
				}
				break;
			
			case "use":
				if(words.Length < 2)
				{
					OutputText("'use' requires at least one argument.");
				}
				else if (words.Length == 2)
				{
					OutputText(world.Use(words[1], ""));
				}
				else if ((words[2] == "at" || words[2] == "on" || words[2] == "with") && words.Length > 3)
				{
					OutputText(world.Use(words[1], words[3]));
				}
				else
				{
					OutputText(world.Use(words[1], words[2]));
				}
				break;
				
			default:
				OutputText("Invalid command.");
				break;
		}
	}
}
