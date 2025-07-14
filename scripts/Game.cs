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
	
	// Constructor, instantiate world objects here
	Game()
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
		"It's a wracker, a transforming multitool. It's almost brand new, but the attached gemstone have been in your family for generations.",
		true);
		world.CreateItemType("Stolen_Power_Cell",
		"An outmode, clockwork generator. A low, hurried ticking and a faint glow suggest that it's still functional.",
		true);
		world.CreateItemType("Rubble",
		"The rubble is sharp but not heavy. It'd be easy to remove with just your hands.",
		false);
		world.CreateItemType("Storage", "It has a simple electronic lock.", false);
		world.CreateItemType("Red_Cable", "It's a red cable.", true);
		world.CreateItemType("Blue_Cable", "It's a blue cable.", true);
		world.CreateItemType("Green_Cable", "It's a green cable.", true);
		world.CreateItemType("Purple_Cable", "It's a purple cable.", true);
		
		// Define uses (required items, produced items, destroyed items, create location, description)
		world.CreateUse(
			["Rubble"], ["Storage"], ["Rubble"], ItemCreateLocation.Room,
			"With little effort the rubble is cleared, revealing a storage box with a simple electronic lock."
		);
		world.CreateUse(
			["Wracker", "Storage"], ["Red_Cable", "Blue_Cable", "Green_Cable", "Purple_Cable"], ["Storage"], ItemCreateLocation.Room,
			"With little effort the rubble is cleared, revealing a storage box with a simple electronic lock."
		);
		
		// Add items to rooms
		world.AddItemToRoom("Rubble", "Main Bridge");
		
		// Add player starting items here (Need to be added in InventoryScreen.cs as well!)
		world.AddItemToPlayer("Wracker");
		world.AddItemToPlayer("Stolen_Power_Cell");
		
		// Create parser object
		parser = new Parser(world.GetItemTypes());
	}
	
	private void OutputText(String text)
	{
		EmitSignal(SignalName.TextOutput, text + "\n");
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
		if (result.Command == Command.Take || result.Command == Command.Use)
		{
			foreach (ItemType item in result.ItemsObtained)
			{
				EmitSignal(SignalName.ModifyInventory, item.Name, true);
			}
			
			foreach (ItemType item in result.ItemsLost)
			{
				EmitSignal(SignalName.ModifyInventory, item.Name, false);
			}
		}
		
		if (result.Command == Command.Move)
		{
			// Update minimap
			GD.Print("Test");
			EmitSignal(SignalName.MapMove, result.Direction.ToString(), world.GetRoomName());
			
			// Play walking sound
			AudioManager.Instance.PlaySFX("walk");
		}
	}
}
