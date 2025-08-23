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
		"what remains of the craft is somehow even more silent than the wasteland " +
		"surrounding it. A mechanical cave, devoid of life.");
		
	Parser parser;
	
		ItemType console;
		ItemUse attachPowerCell;
		ItemUse openDoor;
		ItemUse openStorageBox;
		ItemUse removeRubble;
	
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
		world.CreateRoom("Cramped Hallway",
		"A straight path with only two doors. There wouldn't be any need for empty halls in this type of spacecraft. " +
		"What could it have been used for? " + 
		"There is one door to the [color=7b84ff]north[/color], and one to the [color=7b84ff]south[/color].",
		"You walk into a small and [color=efad42]cramped hallway[/color]. You can see another door, just a short distance ahead."
		);
		
		world.CreateRoom("Heart Chamber",
		"The Mäejrans usually built their [color=efad42]ship bridges[/color] in the middle, " +
		"from where networked idola could operate the machinery. There are no windows, " +
		"but you can see four large doors in each direction. In the middle of the room, " +
		"you can see some kind of control panel.",
		"You enter a large, important-looking chamber. Lucky! This must be the spacecraft's main bridge, " +
		"a room also known as a [color=efad42]heart chamber[/color]. " +
		"There's a large [color=7b84ff]console[/color] with several monitors and panels. You also spot three doors, " +
		"excluding the one you just passed through, at opposing ends of the chamber."
		);
		
		world.CreateRoom("Elevator Shaft",
		"An elevator shaft. What remains of the elevator itself lies about two floors down. " +
		"There's no maintenance ladder either. You could probably survive the fall, " +
		"possibly even without breaking any bones, but how would you get up again? " +
		"Your only option, for now, is to head back from where you came.",
		"As you approach a wide open [color=efad42]elevator shaft[/color], an awful stench belches from the depths. " +
		"At the bottom of the shaft, you spot a cruelly contorted body, lying under a broken [color=7b84ff]ladder[/color]. " +
		"You're relieved that the shadows conceal most of this misfortune, but you can easily deduce that, " +
		"whoever this was, they're now definitely dead."
		);
		
		world.CreateRoom("Captain's Quarters",
		"The [color=efad42]Captain's Quarters[/color]. Must have been cozy, once upon a time. " +
		"The room's only door is to the [color=7b84ff]south[/color].",
		"You walk into room with eerily out-of-place ornaments. " +
		"Perhaps this was the [color=efad42]captain's quarters[/color], some two thousand years ago. " +
		"There are no other doors than the one you entered through."
		);
		
		world.CreateRoom("Strange Panels", 
		"The walls in this room are covered with unfamiliar knobs, levers, and screens. " +
		"To you, it looks like submarine controls, only built in stone and strange metals, " +
		"by people who didn't even have a word for electricity. You see two doors, one to the " +
		"[color=7b84ff]west[/color], and one to the [color=7b84ff]south[/color].",
		"This area looks like some kind of [color=efad42]control room[/color]. You see two doors, " +
		"one to the [color=7b84ff]west[/color], and one to the [color=7b84ff]south[/color]."
		);
		
		world.CreateRoom("Kitchen Alcove",
		"A small compartment for preparing food. You can't see anything resembling cold storage, " +
		"so preserving food might've been to advanced even for the ancients. What a peculiar culture. " +
		"There is only one door, to the [color=7b84ff]north[/color].",
		"You walk into a [color=efad42]small alcove[/color]. There's a stove and a number of broken kitchenware scattered around the room."
		);
		
		// Define connections between rooms (room 1, room 2, direction when moving from room 1 to room 2)
		world.ConnectRooms("Breached Entrance", "Cramped Hallway", Direction.North);
		world.ConnectRooms("Cramped Hallway", "Heart Chamber", Direction.North);
		world.ConnectRooms("Heart Chamber", "Captain's Quarters", Direction.North);
		world.ConnectRooms("Heart Chamber", "Elevator Shaft", Direction.West);
		world.ConnectRooms("Heart Chamber", "Strange Panels", Direction.East);
		world.ConnectRooms("Strange Panels", "Kitchen Alcove", Direction.South);
		
		// Define every unique type of item (item name, item description, can be picked up, is visible [optional], icon path [optional])
		
		world.CreateItemType("Wracker", // Starting inventory
		"It's a [color=38a868]wracker[/color], a transforming multitool. You've rigged it for bypassing " +
		"older, low-leveled passwords and locks. It's almost brand new, " + 
		"but the attached gemstone has been with your family for generations.",
		true, "res://assets/item_multitool_0.png");
		
		world.CreateItemType("Stolen Power Cell", // Starting inventory
		"An outmode, clockwork [color=38a868]generator[/color]. A low, " +
		"hurried ticking and a faint glow suggest that it's still functional.",
		true, "res://assets/item_powercell_0.png");
		
		world.CreateItemType("Carcass", // Room: Breached Entrance
		"The remains of a small animal, possibly a rodent. It might have sought shelter from the sweltering heat, " +
		"but was unable to claw its way out again.",
		false);
		
		world.CreateItemType("Door", // Room: Breached Entrance
		"A mangled, metal [color=7b84ff]door[/color] blocks your way forward.",
		false);
		
		world.CreateItemType("Outside", // Room: Breached Entrance
		"You can see the outside from the hole you entered through. Ripples of heat shimmer cover the red and yellow landscape.",
		false, false);
		
		world.CreateItemType("Wasteland", // Room: Breached Entrance
		"You can see the outside from the hole you entered through. Ripples of heat shimmer cover the red and yellow landscape.",
		false, false);
		
		world.CreateItemType("Debris", // Room: Cramped Hallway
		"There's a large, knife-edged piece of metallic wreckage in the middle of the hallway. " +
		"Part of the floor must have ruptured in the crash. You'd better stay clear of it.",
		false);
		
		world.CreateItemType("Wall", // Room: Cramped Hallway
		"The walls are covered with metre-thick, cylindrical padding.",
		false, false);
		
		world.CreateItemType("Rubble", // Room: Heart Chamber
		"A mound of splintered blackstone and smashed machinery. Some kind of [color=7b84ff]storage box[/color] " + 
		"lies half-buried under the mess.",
		false);
		
		console = world.CreateItemType("Console", // Room: Heart Chamber
		"An antique control panel. The panels around the [color=7b84ff]console[/color] are covered with primitive buttons, " +
		"switches, and sockets for who knows what purpose. One socket in particular catches your eye; " +
		"a [color=38a868]power cell[/color] might fit.",
		false);
		
		world.CreateItemType("Corpse", // Room: Elevator Shaft
		"The corpse of a looter, presumably. You can't make out any distinct features, but it appears to be an adult. " +
		"Judging by the stench, they likely died about a week ago.",
		false);
		
		world.CreateItemType("Mummified Corpse", // Room: Kitchen Alcove
		"You see the [color=efad42]mummified remains[/color] of an unknown person, their bones and shriveled skin mixed with debris. " +
		"Was this person part of the crew, or a looter?",
		false);
		
		world.CreateItemType("Self", // Should be in every room
		"You take a moment to feel yourself wasting away. Better get moving.",
		false, false);
		
		world.CreateItemType("Ladder", // Room: Elevator Shaft
		"You spot bolt holes in the wall where a ladder should be.",
		false, false);
		
		world.CreateItemType("Bolt Holes", // Room: Elevator Shaft
		"Holes in the wall of the elevator shaft, were a maintenance ladder used to be. " +
		"The bolt holes are of no use to you now.",
		false, false);
		
		world.CreateItemType("Body", // Room: Elevator Shaft
		"The corpse of a looter, presumably. You can't make out any distinct features, but it appears to be an adult. " +
		"Judging by the stench, they likely died about a week ago.",
		false, false);
		
		world.CreateItemType("Code Lock", // Room: Heart Chamber
		"A metal door with a keypad terminal is blocking the way east.",
		false);
		
		world.CreateItemType("Note", // Room: Captain's Quarters
		"A small note with the letters '[color=7b84ff]CXXIII[/color]' written on it.",
		false);
		
		world.CreateItemType("Storage", // Room: Heart Chamber
		"A storage box, with a simple [color=7b84ff]electronic lock[/color]. " +
		"The box is made of some heat resistant, lightweight metal. We still haven't been able to replicate anything like it.",
		false);
		world.CreateItemType("Red Cable", "It's a red cable.", true);
		world.CreateItemType("Blue Cable", "It's a blue cable.", true);
		world.CreateItemType("Green Cable", "It's a green cable.", true);
		world.CreateItemType("Purple Cable", "It's a purple cable.", true);
		
		// Define uses (required items, produced items, destroyed items, create location, requires power, description)
		removeRubble = world.CreateUse(
			["Rubble"], ["Storage"], ["Rubble"], ItemCreateLocation.Room,false,
			"With little effort the rubble is cleared, revealing a [color=7b84ff]storage box[/color] with a simple electronic lock."
		);
		
		attachPowerCell = world.CreateUse(
			["Stolen Power Cell", "Console"], [], ["Stolen Power Cell"], ItemCreateLocation.Room, false,
			"You place the power cell into the hatch and attach the cables. There is a small hiss as the ships power returns."
		);
		
		openStorageBox = world.CreateUse(
			["Wracker", "Storage"], ["Red Cable", "Blue Cable", "Green Cable", "Purple Cable"], ["Storage"], ItemCreateLocation.Room, false,
			"With a click and a chime the lock is undone and the box lid opens to reveal a large assortment of coloured [color=38a868]cables[/color]. " + 
			"The box contains green, purple, red, and blue cables."
		);
		
		openDoor = world.CreateUse(
			["Door"], [], ["Door"], ItemCreateLocation.Room, false,
			"Although you're somewhat drained from the journey through the wasteland, you still manage to pry the door open. Phew!"
		);
		
		// Define input actions (required item, producedItems, create location, description on correct input, description on wrong input, required input)
		world.CreateInputAction(
			"Code Lock", [], ItemCreateLocation.Room, true,
			"The terminal beeps approvingly. The door unlocks!", "The terminal beeps in disapprovement. The door remains locked.",
			"123"
		);
		
		// Add items to rooms
		world.AddItemToRoom("Carcass", "Breached Entrance");
		world.AddItemToRoom("Outside", "Breached Entrance"); // Hidden item
		world.AddItemToRoom("Wasteland", "Breached Entrance"); // Hidden item
		world.AddItemToRoom("Debris", "Cramped Hallway");
		world.AddItemToRoom("Wall", "Cramped Hallway"); // Hidden item
		world.AddItemToRoom("Console", "Heart Chamber");
		world.AddItemToRoom("Rubble", "Heart Chamber");
		world.AddItemToRoom("Note", "Captain's Quarters");
		world.AddItemToRoom("Corpse", "Elevator Shaft");
		world.AddItemToRoom("Body", "Elevator Shaft"); // Hidden item
		world.AddItemToRoom("Ladder", "Elevator Shaft"); // Hidden item
		world.AddItemToRoom("Bolt Holes", "Elevator Shaft"); // Hidden item
		world.AddItemToRoom("Mummified Corpse", "Kitchen Alcove");
		
		// 'Self' is a hidden item, added to every room individually
		world.AddItemToRoom("Self", "Breached Entrance");
		world.AddItemToRoom("Self", "Cramped Hallway");
		world.AddItemToRoom("Self", "Heart Chamber");
		world.AddItemToRoom("Self", "Elevator Shaft");
		world.AddItemToRoom("Self", "Captain's Quarters");
		world.AddItemToRoom("Self", "Strange Panels");
		world.AddItemToRoom("Self", "Kitchen Alcove");
		
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
			EmitSignal(SignalName.ModifyInventory, item, world.GetItem(item).IconPath, true);
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
			console.Description = "Some kind of console. You hear the hum of its fan working beneath the casing.";
			AudioManager.Instance.PlaySFX("event_powercell");
		}
		if (result.ItemUse == openDoor)
		{
			AudioManager.Instance.PlaySFX("door_open");
		}
		if (result.ItemUse == openStorageBox)
		{
			AudioManager.Instance.PlaySFX("item_multitool");
		}
		if (result.ItemUse == removeRubble)
		{
			AudioManager.Instance.PlaySFX("pickup_0");
		}
	}
}
