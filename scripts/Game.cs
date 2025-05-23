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
		world.AddRoom("Breached Entrance",
		"The scant beams of sunlight piercing through the broken hull and gives life to the coffin-like silence. Had the ship crashed elsewhere it might've been taken back by nature, but as it stands, the craft remains is somehow even more silent than the wasteland surrounding it. A mechanical cave devoid of life.");
		world.AddRoom("Cramped Hallway", "This room is super red");
		world.AddRoom("Main Bridge", "This room is super red");

		// Set starting room
		world.SetCurrentRoom("Breached Entrance");
		
		// Define connections between rooms (room 1, room 2, direction when moving from room 1 to room 2)
		world.AddConnection("Breached Entrance", "Cramped Hallway", Direction.North);
		world.AddConnection("Cramped Hallway", "Main Bridge", Direction.North);

		// Add items to rooms (item name, item description, room name, can be picked up)
		world.AddItem("rubble",
		"The rubble is sharp but not heavy. It'd be easy to remove with just your hands.",
		"Main Bridge", false);
		
		// Add player starting items here (Need to be added in InventoryScreen.cs as well!)


		world.AddItemToPlayer("Wracker",
		"It's a Wracker, a transforming multitool. It's almost brand new, but the attached gemstone have been in your family for generations.");
		world.AddItemToPlayer("Stolen Power Cell",
		"An outmode, clockwork generator. A low, hurried ticking and a faint glow suggest that it's still functional.");
		
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
			case "grab":
				if(words.Length < 2)
				{
					OutputText("'" + words[1] + "' requires an argument.");
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
			case "walk":
				if(words.Length < 2)
				{
					OutputText("'" + words[1] + "' requires an argument.");
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
						OutputText("There is nowhere to go " + direction + ".");
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
			
			case "help":
				if(words.Length < 2)
				{
					OutputText(">Type [look] or [examine] for a description of an item or your current surroundings.\n>[walk] or [move] must be followed by a direction, such as [north] or [left].\n>[take] or [grab] must be followed by a noun, such as [key] or [gadget].");
				}
				break;
				
			default:
				OutputText("Invalid command.");
				break;			
		}
	}
}
