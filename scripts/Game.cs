using Godot;
using System;
using System.Collections.Generic;

public partial class Game : Node
{
	// Starting room is created here
	World world = new  World("Breached Entrance",
		"The scant beams of sunlight piercing through the broken hull and " +
		"gives life to the coffin-like silence. Had the ship crashed " +
		"elsewhere it might've been taken back by nature, but as it stands, " +
		"the craft remains is somehow even more silent than the wasteland " +
		"surrounding it. A mechanical cave devoid of life.");
	
	[Signal]
	public delegate void TextOutputEventHandler();
	
	[Signal]
	public delegate void MapMoveEventHandler();
	
	[Signal]
	public delegate void ModifyInventoryEventHandler();
	
	// Constructor, instantiate world objects here
	Game()
	{
		// Room and item names must be unique!
		
		// Create rooms (name, descripion), starting room is created further up
		world.CreateRoom("Cramped Hallway", "This room is super red");
		world.CreateRoom("Main Bridge", "This room is super red");
		
		// Define connections between rooms (room 1, room 2, direction when moving from room 1 to room 2)
		world.ConnectRooms("Breached Entrance", "Cramped Hallway", Direction.North);
		world.ConnectRooms("Cramped Hallway", "Main Bridge", Direction.North);

		// Define every unique type of item (item name, item description, can be picked up)
		world.CreateItemType("Rubble",
		"The rubble is sharp but not heavy. It'd be easy to remove with just your hands.",
		false);
		world.CreateItemType("Wracker",
		"It's a Wracker, a transforming multitool. It's almost brand new, but the attached gemstone have been in your family for generations.",
		true);
		world.CreateItemType("Stolen Power Cell",
		"An outmode, clockwork generator. A low, hurried ticking and a faint glow suggest that it's still functional.",
		true);
		
		// Add items to rooms
		world.AddItemToRoom("Rubble", "Main Bridge");
		
		// Add player starting items here (Need to be added in InventoryScreen.cs as well!)
		world.AddItemToPlayer("Wracker");
		world.AddItemToPlayer("Stolen Power Cell");
	}
	
	private void OutputText(String text)
	{
		EmitSignal(SignalName.TextOutput, text + "\n");
	}
	
	public void PlayerInputReceived(String textInput)
	{
		// Echo input in output window
		OutputText(">" + textInput);
	}
}
