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
		
	Parser parser;
	
	[Signal]
	public delegate void TextOutputEventHandler();
	
	[Signal]
	public delegate void MapMoveEventHandler();
	
	[Signal]
	public delegate void ModifyInventoryEventHandler();
	
	[Signal]
	public delegate void GenerateMapTileEventHandler();
	
	// Instantiate world objects here
	public override void _Ready()
	{
		// Room and item names must be unique!
		
		// Create rooms (name, descripion), starting room is created further up
		world.CreateRoom("Cramped Hallway", "This room is super red.");
		world.CreateRoom("Main Bridge", "This room is super red.");
		
		// Define connections between rooms (room 1, room 2, direction when moving from room 1 to room 2)
		world.ConnectRooms("Breached Entrance", "Cramped Hallway", Direction.North);
		world.ConnectRooms("Cramped Hallway", "Main Bridge", Direction.North);

		// Define every unique type of item (item name, item description, can be picked up)
		world.CreateItemType("Wracker",
		"It's a [color=38a868]wracker[/color], a transforming multitool. It's almost brand new, but the attached gemstone have been in your family for generations.",
		true);
		world.CreateItemType("Stolen_Power_Cell",
		"An outmode, clockwork [color=38a868]generator[/color]. A low, hurried ticking and a faint glow suggest that it's still functional.",
		true);
		world.CreateItemType("Rubble",
		"A mound of splintered blackstone and smashed machinery. Some kind of [color=7b84ff]container[/color] lies half-buried under the mess.",
		false);
		world.CreateItemType("Carcass",
		"The remains of a small animal, possibly a rodent. It might have sought shelter from the sweltering heat, " +
		"but was unable to claw its way out again.",
		false);
		world.CreateItemType("Wasteland",
		"This item shouldn't be listed by the look command!",
		false);
		world.CreateItemType("Door",
		"A metal door blocks your path.",
		false);
		world.CreateItemType("Storage", "It has a simple electronic lock.", false);
		world.CreateItemType("Red_Cable", "It's a red cable.", true);
		world.CreateItemType("Blue_Cable", "It's a blue cable.", true);
		world.CreateItemType("Green_Cable", "It's a green cable.", true);
		world.CreateItemType("Purple_Cable", "It's a purple cable.", true);
		
		// Define uses (required items, produced items, destroyed items, create location, description)
		world.CreateUse(
			["Rubble"], ["Storage"], ["Rubble"], ItemCreateLocation.Room,
			"With little effort the rubble is cleared, revealing a [color=7b84ff]storage box[/color] with a simple electronic lock."
		);
		world.CreateUse(
			["Wracker", "Storage"], ["Red_Cable", "Blue_Cable", "Green_Cable", "Purple_Cable"], ["Storage"], ItemCreateLocation.Room,
			"With a click and a chime the lock is undone and the box lid opens to reveal a large assortment of coloured cables. The box contains green, purple, red, and blue cables."
		);
		world.CreateUse(
			["Door"], [], ["Door"], ItemCreateLocation.Room,
			"You open the door without difficulty."
		);
		
		// Add items to rooms
		world.AddItemToRoom("Rubble", "Main Bridge");
		world.AddItemToRoom("Carcass", "Breached Entrance");
		world.AddItemToRoom("Wasteland", "Breached Entrance");
		
		// Add items as obstacles between rooms (item, room, direction to block)
		world.AddItemAsObstacle("Door", "Breached Entrance", Direction.North);
		
		// Add items to player inventory
		AddStartingItems(["Wracker", "Stolen_Power_Cell"]);
		
		// Create parser object
		parser = new Parser(world.GetItemTypes());
		
		// Generate room tiles in minimap
		foreach (TileCoordinate coord in world.GenerateTileCoordinates())
		{
			EmitSignal(SignalName.GenerateMapTile, coord.Name, coord.Position);	
		}
	}
	
	private void OutputText(String text)
	{
		EmitSignal(SignalName.TextOutput, text + "\n");
	}
	
	private void AddStartingItems(String[] itemNames)
	{
		foreach (String item in itemNames)
		{
			world.AddItemToPlayer(item);
			EmitSignal(SignalName.ModifyInventory, item, true);
		}
	}
	
	public void PlayerInputReceived(String textInput)
	{
		// Echo input in output window
		OutputText(">" + textInput + "\n");
	
		// Parse input
		CommandInput input = parser.GetCommand(textInput);
		
		// Execute command
		CommandOutput result = world.ExecuteCommand(input);
		
		// Output text
		OutputText(result.Text + "\n");
		
		// Modify inventory screen if necessary
		foreach (ItemType item in result.ItemsObtained)
		{
			EmitSignal(SignalName.ModifyInventory, item.Name, true);
		}
		foreach (ItemType item in result.ItemsLost)
		{
			EmitSignal(SignalName.ModifyInventory, item.Name, false);
		}
		
		if (result.Command == Command.Move)
		{
			// Update minimap
			EmitSignal(SignalName.MapMove, world.GetRoomName());
			
			// Play walking sound
			AudioManager.Instance.PlaySFX("walk");
		}
	}
}
