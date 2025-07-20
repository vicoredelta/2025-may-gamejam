using Godot;
using System;
using System.Collections.Generic;
using Godot.Collections;

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
	
	ItemUse attachPowerCell;
	
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
		
		// Create rooms (name, descripion, first time description [optional]), starting room is created further up
		world.CreateRoom("Cramped Hallway", "A straight path ahead. There wouldn’t be any need for empty halls in this type of spacecraft. What could it have been used for?");
		world.CreateRoom("Heart Chamber", "The Mäejrans usually built their ship bridges in the middle, " +
		"from where networked idola could operate the machinery. There are no windows, " +
		"but you can see four large [color=7b84ff]doors[/color] in each direction. In the middle of the room, " +
		"you can see some kind of [color=7b84ff]control panel[/color]. You also spot a large pile of [color=7b84ff]rubble[/color] in a corner.");
		world.CreateRoom("Elevator Shaft", "Don't fall down!");
		world.CreateRoom("Captain's Quarters", "Must have been cozy, once upon a time.");
		world.CreateRoom("Strange Panels", "What could they have been used for?");
		
		// Define connections between rooms (room 1, room 2, direction when moving from room 1 to room 2)
		world.ConnectRooms("Breached Entrance", "Cramped Hallway", Direction.North);
		world.ConnectRooms("Cramped Hallway", "Heart Chamber", Direction.North);
		world.ConnectRooms("Heart Chamber", "Elevator Shaft", Direction.West);
		world.ConnectRooms("Heart Chamber", "Strange Panels", Direction.East);
		world.ConnectRooms("Heart Chamber", "Captain's Quarters", Direction.North);

		// Define every unique type of item (item name, item description, can be picked up, is visible [optional], icon path [optional])
		world.CreateItemType("Wracker",
		"It's a [color=38a868]wracker[/color], a transforming multitool. It's almost brand new, but the attached gemstone have been in your family for generations.",
		true);
		world.CreateItemType("Stolen Power Cell",
		"An outmode, clockwork [color=38a868]generator[/color]. A low, hurried ticking and a faint glow suggest that it's still functional.",
		true);
		world.CreateItemType("Rubble",
		"A mound of splintered blackstone and smashed machinery. Some kind of [color=7b84ff]Inventory[/color] lies half-buried under the mess.",
		false);
		world.CreateItemType("Console",
		"Some kind of console. There is a open hatch under it that seem to be where the power source used to be placed...",
		false);
		world.CreateItemType("Powered Console",
		"Some kind of console. You hear the humm of its fan working beneath the casing.",
		false);
		world.CreateItemType("Carcass",
		"The remains of a small animal, possibly a rodent. It might have sought shelter from the sweltering heat, " +
		"but was unable to claw its way out again.",
		false);
		world.CreateItemType("Wasteland",
		"This item shouldn't be listed by the look command!",
		false, false);
		world.CreateItemType("Door",
		"A metal door blocks your path.",
		false);
		world.CreateItemType("Code Lock",
		"A metal door with a small terminal is blocking the way. There is a small post-it note with the numbers '123' written on it.",
		false);
		world.CreateItemType("Storage", "It has a simple electronic lock.", false);
		world.CreateItemType("Red Cable", "It's a red cable.", true);
		world.CreateItemType("Blue Cable", "It's a blue cable.", true);
		world.CreateItemType("Green Cable", "It's a green cable.", true);
		world.CreateItemType("Purple Cable", "It's a purple cable.", true);
		
		// Define uses (required items, produced items, destroyed items, create location, requires power, description)
		world.CreateUse(
			["Rubble"], ["Storage"], ["Rubble"], ItemCreateLocation.Room,false,
			"With little effort the rubble is cleared, revealing a [color=7b84ff]storage box[/color] with a simple electronic lock."
		);
		attachPowerCell = world.CreateUse(
			["Stolen Power Cell", "Console"], ["Powered Console"], ["Console", "Stolen Power Cell"], ItemCreateLocation.Room, false,
			"You place the powercell into the hatch and attach the cables. There is a small hiss as the ships power returns."
		);
		world.CreateUse(
			["Wracker", "Storage"], ["Red Cable", "Blue Cable", "Green Cable", "Purple Cable"], ["Storage"], ItemCreateLocation.Room, false,
			"With a click and a chime the lock is undone and the box lid opens to reveal a large assortment of coloured cables. The box contains green, purple, red, and blue cables."
		);
		world.CreateUse(
			["Door"], [], ["Door"], ItemCreateLocation.Room, false,
			"You open the door without difficulty."
		);
		
		// Define input actions (required item, producedItems, create location, description on correct input, description on wrong input, required input)
		world.CreateInputAction(
			"Code Lock", [], ItemCreateLocation.Room, true,
			"The door opens.", "The remains closed with a dissapproving beep.",
			"123"
		);
		
		// Add items to rooms
		world.AddItemToRoom("Rubble", "Heart Chamber");
		world.AddItemToRoom("Console", "Heart Chamber");
		world.AddItemToRoom("Carcass", "Breached Entrance");
		world.AddItemToRoom("Wasteland", "Breached Entrance");
		
		// Add items as obstacles between rooms (item, room, direction to block)
		world.AddItemAsObstacle("Door", "Breached Entrance", Direction.North);
		world.AddItemAsObstacle("Code Lock", "Heart Chamber", Direction.East);
		
		// Add items to player inventory
		AddStartingItems(["Wracker", "Stolen Power Cell"]);
		
		// Create parser object
		parser = new Parser(world.GetItemTypes());
		
		// Generate room tiles in minimap
		foreach (TileCoordinate coord in world.GenerateTileCoordinates())
		{
			EmitSignal(SignalName.GenerateMapTile, coord.Name, coord.Position);	
		}
		//sets minimap visibility
		var visitedStatus = world.GetVisitedStatusForAllRooms();
		var godotDict = new Godot.Collections.Dictionary<string, bool>();

		foreach (var kvp in visitedStatus)
		{
			godotDict[kvp.Key] = kvp.Value;
		}
		EmitSignal(SignalName.MapMove, world.GetRoomName(), godotDict);
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
			EmitSignal(SignalName.ModifyInventory, item, "", true);
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
			EmitSignal(SignalName.ModifyInventory, item.Name, item.IconPath, true);
		}
		foreach (ItemType item in result.ItemsLost)
		{
			EmitSignal(SignalName.ModifyInventory, item.Name, item.IconPath, false);
		}
		
		if (result.Command == Command.Move)
		{
			// Update minimap
			var visitedStatus = world.GetVisitedStatusForAllRooms();
			var godotDict = new Godot.Collections.Dictionary<string, bool>();

				foreach (var kvp in visitedStatus)
			{
				godotDict[kvp.Key] = kvp.Value;
			}
			EmitSignal(SignalName.MapMove, world.GetRoomName(), godotDict);
			
			// Play walking sound
			AudioManager.Instance.PlaySFX("walk");
		}
		
		// Use specific happenings
		if (result.ItemUse == attachPowerCell)
		{
			world.IsPowerOn = true;
		}
	}
}
