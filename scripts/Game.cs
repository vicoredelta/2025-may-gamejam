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
		
	}
}
