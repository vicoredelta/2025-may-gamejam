using Godot;
using System;
using System.Collections.Generic;
using Godot.Collections;

public partial class Game : Node
{
	ItemType console;
	ItemType redRods;
	ItemType blueRods;
	ItemType greenRods;
	UseAction attachPowerCell;
	UseAction openDoor;
	UseAction openStorageBox;
	UseAction removeRubble;
	UseAction useRedRods;
	UseAction useBlueRods;
	UseAction useGreenRods;
	UseAction stasisUnlock;
	UseAction getKeycard;
	int invalidCommandCount = 0;
	
	[Signal]
	public delegate void TextOutputEventHandler();
	
	[Signal]
	public delegate void MapMoveEventHandler();
	
	[Signal]
	public delegate void UpdateInventoryEventHandler();
	
	[Signal]
	public delegate void GenerateMapTileEventHandler();
	
	[Signal]
	public delegate void ArrowUpdateEventHandler();
	
	// Instantiate world objects here
	public override void _Ready()
	{
		// Room and item names must be unique!
		
		// Create rooms (name, descripion, first time description [optional])
		World.Instance.CreateRoom("Breached Entrance",
		"The scant beams of sunlight piercing through the broken hull and " +
		"gives life to the coffin-like silence. Had the ship crashed " +
		"elsewhere it might've been taken back by nature, but as it stands, " +
		"what remains of the craft is somehow even more silent than the wasteland " +
		"surrounding it. A mechanical cave, devoid of life."
		);
		
		World.Instance.CreateRoom("Cramped Hallway",
		"A straight path with only two doors. There wouldn't be any need for empty halls in this type of spacecraft. " +
		"What could it have been used for? " + 
		"There is one door to the [color=7b84ff]north[/color], and one to the [color=7b84ff]south[/color].",
		"You walk into a small and [color=efad42]cramped hallway[/color]. You can see another door, just a short distance ahead."
		);
		
		World.Instance.CreateRoom("Heart Chamber",
		"The Mäejrans usually built their [color=efad42]ship bridges[/color] in the middle, " +
		"from where networked idola could operate the machinery. There are no windows, " +
		"but you can see four large doors in each direction. In the middle of the room, " +
		"you can see some kind of control panel.",
		"You enter a large, important-looking chamber. Lucky! This must be the spacecraft's main bridge, " +
		"a room also known as a [color=efad42]heart chamber[/color]. " +
		"There's a large [color=7b84ff]console[/color] with several monitors and panels. You also spot three doors, " +
		"excluding the one you just passed through, at opposing ends of the chamber."
		);
		
		World.Instance.CreateRoom("Elevator Shaft",
		"An elevator shaft. What remains of the elevator itself lies about two floors down. " +
		"There's no maintenance ladder either. You could probably survive the fall, " +
		"possibly even without breaking any bones, but how would you get up again? " +
		"Your only option, for now, is to head back from where you came.",
		"As you approach a wide open [color=efad42]elevator shaft[/color], an awful stench belches from the depths. " +
		"At the bottom of the shaft, you spot a cruelly contorted body, lying under a broken [color=7b84ff]ladder[/color]. " +
		"You're relieved that the shadows conceal most of this misfortune, but you can easily deduce that, " +
		"whoever this was, they're now definitely dead."
		);
		
		World.Instance.CreateRoom("Captain's Quarters",
		"The [color=efad42]Captain's Quarters[/color]. Must have been cozy, once upon a time. " +
		"The room's only door is to the [color=7b84ff]south[/color].",
		"You walk into room with eerily out-of-place ornaments. " +
		"Perhaps this was the [color=efad42]captain's quarters[/color], some two thousand years ago. " +
		"There are no other doors than the one you entered through."
		);
		
		World.Instance.CreateRoom("Strange Panels", 
		"The walls in this room are covered with unfamiliar knobs, levers, and screens. " +
		"To you, it looks like submarine controls, only built in stone and strange metals, " +
		"by people who didn't even have a word for electricity. You see two doors, one to the " +
		"[color=7b84ff]west[/color], and one to the [color=7b84ff]south[/color].",
		"This area looks like some kind of [color=efad42]control room[/color]. You see two doors, " +
		"one to the [color=7b84ff]west[/color], and one to the [color=7b84ff]south[/color]."
		);
		
		World.Instance.CreateRoom("Kitchen Alcove",
		"A small compartment for preparing food. You can't see anything resembling cold storage, " +
		"so preserving food might've been too advanced even for the ancients. What a peculiar culture. " +
		"There is only one door, to the [color=7b84ff]north[/color].",
		"You walk into a [color=efad42]small alcove[/color]. There's a stove and a number of broken kitchenware scattered around the room."
		);
		
		World.Instance.CreateRoom("Stasis Pods",
		"There are ten [color=7b84ff]stasis pods[/color], each carrying a long since passed away passenger. They are numbered  1 to 10.",
		"The room is long and slim with large mechanical pods along the western wall. "+
		"The pods have a faint glow to them and eminate a slight chill, likely due to the power being restored. "+
		"There is an open door on the [color=7b84ff]eastern[/color] wall."
		);
		
		World.Instance.CreateRoom("Stasis Control Room",
		"The stasis cells are controlled from this room."
		);
		
		// Set starting room
		World.Instance.SetCurrentRoom("Breached Entrance");
		
		// Define connections between rooms (room 1, room 2, direction when moving from room 1 to room 2)
		World.Instance.ConnectRooms("Breached Entrance", "Cramped Hallway", Direction.North);
		World.Instance.ConnectRooms("Cramped Hallway", "Heart Chamber", Direction.North);
		World.Instance.ConnectRooms("Heart Chamber", "Stasis Pods", Direction.North);
		World.Instance.ConnectRooms("Heart Chamber", "Elevator Shaft", Direction.West);
		World.Instance.ConnectRooms("Heart Chamber", "Strange Panels", Direction.East);
		World.Instance.ConnectRooms("Strange Panels", "Kitchen Alcove", Direction.South);
		World.Instance.ConnectRooms("Stasis Pods", "Stasis Control Room", Direction.North);
		World.Instance.ConnectRooms("Stasis Pods", "Captain's Quarters", Direction.East);
		
		// Define every unique type of item (item name, name aliases, item description, can be picked up, is visible [optional], icon path [optional])
		
		World.Instance.CreateItemType("Wracker", [], // Starting inventory
		"It's a [color=38a868]wracker[/color], a transforming multitool. You've rigged it for bypassing " +
		"older, low-leveled passwords and locks. It's almost brand new, " + 
		"but the attached gemstone has been with your family for generations.",
		true, "res://assets/item_multitool_0.png");
		
		World.Instance.CreateItemType("Stolen Power Cell", ["Power Cell"], // Starting inventory
		"An outmode, clockwork [color=38a868]generator[/color]. A low, " +
		"hurried ticking and a faint glow suggest that it's still functional.",
		true, "res://assets/item_powercell_0.png");
		
		World.Instance.CreateItemType("Carcass", [], // Room: Breached Entrance
		"The remains of a small animal, possibly a rodent. It might have sought shelter from the sweltering heat, " +
		"but was unable to claw its way out again.",
		false);
		
		World.Instance.CreateItemType("Door", [], // Room: Breached Entrance
		"A mangled, metal [color=7b84ff]door[/color] blocks your way forward.",
		false);
		
		World.Instance.CreateItemType("Outside", [], // Room: Breached Entrance
		"You can see the outside from the hole you entered through. Ripples of heat shimmer cover the red and yellow landscape.",
		false, false);
		
		World.Instance.CreateItemType("Down", [], // In every room
		"The floor doesn't give off any murderous intent.",
		false, false);
		
		World.Instance.CreateItemType("Up", [], // In every room
		"The ceiling. Thankfully taller than you.",
		false, false);
		
		World.Instance.CreateItemType("Wasteland", [], // Room: Breached Entrance
		"You can see the outside from the hole you entered through. Ripples of heat shimmer cover the red and yellow landscape.",
		false, false);
		
		World.Instance.CreateItemType("Debris", [], // Room: Cramped Hallway
		"There's a large, knife-edged piece of metallic wreckage in the middle of the hallway. " +
		"Part of the floor must have ruptured in the crash. You'd better stay clear of it.",
		false);
		
		World.Instance.CreateItemType("Wall", [], // Room: Cramped Hallway
		"The walls are covered with metre-thick, cylindrical padding.",
		false, false);
		
		World.Instance.CreateItemType("Rubble", [], // Room: Heart Chamber
		"A mound of splintered blackstone and smashed machinery. Some kind of [color=7b84ff]storage box[/color] " + 
		"lies half-buried under the mess.",
		false);
		
		console = World.Instance.CreateItemType("Console", [], // Room: Heart Chamber
		"An antique control panel. The panels around the [color=7b84ff]console[/color] are covered with primitive buttons, " +
		"switches, and sockets for who knows what purpose. One socket in particular catches your eye; " +
		"a [color=38a868]power cell[/color] might fit.",
		false);
		
		World.Instance.CreateItemType("Corpse", [], // Room: Elevator Shaft
		"The corpse of a looter, presumably. You can't make out any distinct features, but it appears to be an adult. " +
		"Judging by the stench, they likely died about a week ago.",
		false);
		
		World.Instance.CreateItemType("Mummified Corpse", ["Corpse"], // Room: Kitchen Alcove
		"You see the [color=efad42]mummified remains[/color] of an unknown person, their shriveled skin and mixed with debris. " +
		"Was this person part of the crew, or a looter?",
		false);
		
		World.Instance.CreateItemType("Self", [], // Should be in every room
		"You take a moment to feel yourself wasting away. Better get moving.",
		false, false);
		
		World.Instance.CreateItemType("Ladder", [], // Room: Elevator Shaft
		"You spot bolt holes in the wall where a ladder should be.",
		false, false);
		
		World.Instance.CreateItemType("Bolt Holes", ["Hole"], // Room: Elevator Shaft
		"Holes in the wall of the elevator shaft, were a maintenance ladder used to be. " +
		"The bolt holes are of no use to you now.",
		false, false);
		
		World.Instance.CreateItemType("Body", [], // Room: Elevator Shaft
		"The corpse of a looter, presumably. You can't make out any distinct features, but it appears to be an adult. " +
		"Judging by the stench, they likely died about a week ago.",
		false, false);
		
		World.Instance.CreateItemType("Code Lock", ["Lock"], // Room: Heart Chamber
		"A metal door with a keypad terminal is blocking the way east.",
		false);
		
		World.Instance.CreateItemType("Note", [], // Room: Captain's Quarters
		"A small note with the letters '[color=7b84ff]CXXIII[/color]' written on it.",
		false);
		
		World.Instance.CreateItemType("Storage", ["Storage Box", "Box", "Electronic Lock"], // Room: Heart Chamber
		"A storage box, with a simple [color=7b84ff]electronic lock[/color]. " +
		"The box is made of some heat resistant, lightweight metal. We still haven't been able to replicate anything like it.",
		false);
		
		redRods = World.Instance.CreateItemType("Red Rods", ["Red", "Red Rod"],
		"A handful of red stone rods. They carry an uncomfortably high current.", 
		true, "res://assets/item_rod_red.png");
		blueRods = World.Instance.CreateItemType("Blue Rods", ["Blue", "Blue Rod"],
		"A handful of blue stone rods. They carry a medium current, mildly irritating to the touch.",
		true, "res://assets/item_rod_blue.png");
		greenRods = World.Instance.CreateItemType("Green Rods", ["Green", "Green Rod"], 
		"A handful of green stone rods. They carry a low current.",
		true, "res://assets/item_rod_green.png");
		
		// Items for stasis puzzle
		World.Instance.CreateItemType("Stasis Control Terminal", ["Terminal"], 
		"A stasis pod containing the remains of a passenger, long since passed away.", false);
		World.Instance.CreateItemType("Stasis Pod 1", ["Pod 1", "1"], 
		"A stasis pod containing the remains of a passenger, long since passed away.", false, false);
		World.Instance.CreateItemType("Stasis Pod 2", ["Pod 2", "2"], 
		"A stasis pod containing the remains of a passenger, long since passed away.", false, false);
		World.Instance.CreateItemType("Stasis Pod 3", ["Pod 3", "3"], 
		"A stasis pod containing the remains of a passenger, long since passed away.", false, false);
		World.Instance.CreateItemType("Stasis Pod 4", ["Pod 4", "4"], 
		"A stasis pod containing the remains of a passenger, long since passed away.", false, false);
		World.Instance.CreateItemType("Stasis Pod 5", ["Pod 5", "5"], 
		"A stasis pod containing the remains of a passenger, long since passed away.", false, false);
		World.Instance.CreateItemType("Stasis Pod 6", ["Pod 6", "6"], 
		"A stasis pod containing the remains of a passenger, long since passed away.", false, false);
		World.Instance.CreateItemType("Stasis Pod 7", ["Pod 7", "7"], 
		"A stasis pod containing the remains of a passenger, long since passed away.", false, false);
		World.Instance.CreateItemType("Stasis Pod 8", ["Pod 8", "8"], 
		"A stasis pod containing the remains of a passenger, long since passed away.", false, false);
		World.Instance.CreateItemType("Stasis Pod 9", ["Pod 9", "9"], 
		"A stasis pod containing the remains of a passenger, long since passed away.", false, false);
		World.Instance.CreateItemType("Stasis Pod 10", ["Pod 10", "10"], 
		"A stasis pod containing the remains of a passenger, long since passed away.", false, false);
		World.Instance.CreateItemType("Captain's Keycard", ["Keycard"], 
		"The keycard belonging to the captain of this ship.", true);
		
		// Define uses (required items, produced items, destroyed items, create location, description, requires power on [optional], requires power cell [optional])
		removeRubble = World.Instance.CreateUse(
			["Rubble"], ["Storage"], ["Rubble"], ItemCreateLocation.Room,
			"With little effort the rubble is cleared, revealing a [color=7b84ff]storage box[/color] with a simple electronic lock."
		);
		
		attachPowerCell = World.Instance.CreateUse(
			["Stolen Power Cell", "Console"], [], ["Stolen Power Cell"], ItemCreateLocation.Room,
			"You place the power cell into the hatch."
		);
		
		openStorageBox = World.Instance.CreateUse(
			["Wracker", "Storage"], ["Red Rods", "Blue Rods", "Green Rods"], ["Storage"], ItemCreateLocation.Room,
			"With a click and a chime the lock is undone and the box lid opens to reveal a large assortment of coloured rods. " + 
			"The box contains [color=38a868]red[/color], [color=38a868]green[/color], and [color=38a868]blue rods[/color]."
		);
		
		useRedRods = World.Instance.CreateUse(
			["Red Rods", "Console"], [], [], ItemCreateLocation.Room,
			"You use one of the red rods to supply some power to the ship.", requiresCell: true
		);
		
		useBlueRods = World.Instance.CreateUse(
			["Blue Rods", "Console"], [], [], ItemCreateLocation.Room,
			"You use one of the blue rods to supply some power to the ship.", requiresCell: true
		);
		
		useGreenRods = World.Instance.CreateUse(
			["Green Rods", "Console"], [], [], ItemCreateLocation.Room,
			"You use one of the green rods to supply some power to the ship.", requiresCell: true
		);
		
		openDoor = World.Instance.CreateUse(
			["Door"], [], ["Door"], ItemCreateLocation.Room,
			"Although you're somewhat drained from the journey through the wasteland, you still manage to pry the door open. Phew!"
		);
		
		stasisUnlock = World.Instance.CreateUse(
			["Stasis Control Terminal"], [], [], ItemCreateLocation.Room,
			stasisUnlockDescription
		);
		
		// Uses for Stasis puzzle
		String wrongPodDescription = "You search the corpse thouroughly, it did not contain the captain's keycard. The stasis pods close afterwards";
		World.Instance.CreateUse(
			["Stasis Pod 1"], [], [], ItemCreateLocation.Player,
			wrongPodDescription, requiresStasisUnlock: true 
		);
		World.Instance.CreateUse(
			["Stasis Pod 2"], [], [], ItemCreateLocation.Player,
			wrongPodDescription, requiresStasisUnlock: true 
		);
		World.Instance.CreateUse(
			["Stasis Pod 3"], [], [], ItemCreateLocation.Player,
			wrongPodDescription, requiresStasisUnlock: true 
		);
		World.Instance.CreateUse(
			["Stasis Pod 4"], [], [], ItemCreateLocation.Player,
			wrongPodDescription, requiresStasisUnlock: true 
		);
		getKeycard = World.Instance.CreateUse(
			["Stasis Pod 5"], ["Captain's Keycard"], [], ItemCreateLocation.Player,
			"You search the corpse and find the captain's keycard.", requiresStasisUnlock: true 
		);
		World.Instance.CreateUse(
			["Stasis Pod 6"], [], [], ItemCreateLocation.Player,
			wrongPodDescription, requiresStasisUnlock: true 
		);
		World.Instance.CreateUse(
			["Stasis Pod 7"], [], [], ItemCreateLocation.Player,
			wrongPodDescription, requiresStasisUnlock: true 
		);
		World.Instance.CreateUse(
			["Stasis Pod 8"], [], [], ItemCreateLocation.Player,
			wrongPodDescription, requiresStasisUnlock: true 
		);
		World.Instance.CreateUse(
			["Stasis Pod 9"], [], [], ItemCreateLocation.Player,
			wrongPodDescription, requiresStasisUnlock: true 
		);
		World.Instance.CreateUse(
			["Stasis Pod 10"], [], [], ItemCreateLocation.Player,
			wrongPodDescription, requiresStasisUnlock: true 
		);
		World.Instance.CreateUse(
			["Stasis Pod 10"], [], [], ItemCreateLocation.Player,
			wrongPodDescription, requiresStasisUnlock: true 
		);
		
		// Define input actions (required item, producedItems, create location, description on correct input, description on wrong input, required input)
		World.Instance.CreateInputAction(
			"Code Lock", [], ItemCreateLocation.Room,
			"The terminal beeps approvingly. The door unlocks!", "The terminal beeps in disapprovement. The door remains locked.",
			"123", requiresPower: true
		);
		
		// Add items to rooms
		World.Instance.AddItemToRoom("Carcass", "Breached Entrance");
		World.Instance.AddItemToRoom("Outside", "Breached Entrance"); // Hidden item
		World.Instance.AddItemToRoom("Wasteland", "Breached Entrance"); // Hidden item
		World.Instance.AddItemToRoom("Debris", "Cramped Hallway");
		World.Instance.AddItemToRoom("Wall", "Cramped Hallway"); // Hidden item
		World.Instance.AddItemToRoom("Console", "Heart Chamber");
		World.Instance.AddItemToRoom("Rubble", "Heart Chamber");
		World.Instance.AddItemToRoom("Note", "Captain's Quarters");
		World.Instance.AddItemToRoom("Corpse", "Elevator Shaft");
		World.Instance.AddItemToRoom("Body", "Elevator Shaft"); // Hidden item
		World.Instance.AddItemToRoom("Ladder", "Elevator Shaft"); // Hidden item
		World.Instance.AddItemToRoom("Bolt Holes", "Elevator Shaft"); // Hidden item
		World.Instance.AddItemToRoom("Mummified Corpse", "Kitchen Alcove");
		World.Instance.AddItemToRoom("Stasis Control Terminal", "Stasis Control Room");
		
		// Pods for stasis puzzle
		World.Instance.AddItemToRoom("Stasis Pod 1", "Stasis Pods");
		World.Instance.AddItemToRoom("Stasis Pod 2", "Stasis Pods");
		World.Instance.AddItemToRoom("Stasis Pod 3", "Stasis Pods");
		World.Instance.AddItemToRoom("Stasis Pod 4", "Stasis Pods");
		World.Instance.AddItemToRoom("Stasis Pod 5", "Stasis Pods");
		World.Instance.AddItemToRoom("Stasis Pod 6", "Stasis Pods");
		World.Instance.AddItemToRoom("Stasis Pod 7", "Stasis Pods");
		World.Instance.AddItemToRoom("Stasis Pod 8", "Stasis Pods");
		World.Instance.AddItemToRoom("Stasis Pod 9", "Stasis Pods");
		World.Instance.AddItemToRoom("Stasis Pod 10", "Stasis Pods");
		
		// Add items as obstacles between rooms (item, room, direction to block)
		World.Instance.AddItemAsObstacle("Door", "Breached Entrance", Direction.North);
		World.Instance.AddItemAsObstacle("Code Lock", "Heart Chamber", Direction.East);
		
		// Add items to player inventory
		AddStartingItems(["Wracker", "Stolen Power Cell"]);
		
				// 'Self' is a hidden item, added to every room individually
		World.Instance.AddItemToRoom("Self", "Breached Entrance");
		World.Instance.AddItemToRoom("Self", "Cramped Hallway");
		World.Instance.AddItemToRoom("Self", "Heart Chamber");
		World.Instance.AddItemToRoom("Self", "Elevator Shaft");
		World.Instance.AddItemToRoom("Self", "Captain's Quarters");
		World.Instance.AddItemToRoom("Self", "Strange Panels");
		World.Instance.AddItemToRoom("Self", "Kitchen Alcove");
		World.Instance.AddItemToRoom("Self", "Stasis Pods");
		World.Instance.AddItemToRoom("Self", "Stasis Control Room");
		
		// 'Down' is a hidden item, added to every room individually
		World.Instance.AddItemToRoom("Down", "Breached Entrance");
		World.Instance.AddItemToRoom("Down", "Cramped Hallway");
		World.Instance.AddItemToRoom("Down", "Heart Chamber");
		World.Instance.AddItemToRoom("Down", "Elevator Shaft");
		World.Instance.AddItemToRoom("Down", "Captain's Quarters");
		World.Instance.AddItemToRoom("Down", "Strange Panels");
		World.Instance.AddItemToRoom("Down", "Kitchen Alcove");
		World.Instance.AddItemToRoom("Down", "Stasis Pods");
		World.Instance.AddItemToRoom("Down", "Stasis Control Room");
		
		// 'Up' is a hidden item, added to every room individually
		World.Instance.AddItemToRoom("Up", "Breached Entrance");
		World.Instance.AddItemToRoom("Up", "Cramped Hallway");
		World.Instance.AddItemToRoom("Up", "Heart Chamber");
		World.Instance.AddItemToRoom("Up", "Elevator Shaft");
		World.Instance.AddItemToRoom("Up", "Captain's Quarters");
		World.Instance.AddItemToRoom("Up", "Strange Panels");
		World.Instance.AddItemToRoom("Up", "Kitchen Alcove");
		World.Instance.AddItemToRoom("Up", "Stasis Pods");
		World.Instance.AddItemToRoom("Up", "Stasis Control Room");
		
		// Generate room tiles in minimap
		foreach (TileCoordinate coord in Player.Instance.CurrentRoom.GenerateTileCoordinates())
		{
			EmitSignal(SignalName.GenerateMapTile, coord.Name, coord.Position);	
		}
		//sets minimap visibility
		var visitedStatus = World.Instance.GetVisitedStatusForAllRooms();
		var godotDict = new Godot.Collections.Dictionary<string, bool>();

		foreach (var kvp in visitedStatus)
		{
			godotDict[kvp.Key] = kvp.Value;
		}
		EmitSignal(SignalName.MapMove, Player.Instance.CurrentRoom.Name, godotDict);
		EmitSignal(SignalName.ArrowUpdate);
	}
	
	private void OutputText(String text)
	{
		EmitSignal(SignalName.TextOutput, text + "\n");
	}
	
	private void AddStartingItems(String[] itemNames)
	{
		foreach (String item in itemNames)
		{
			World.Instance.AddItemToPlayer(item);
		}
		
		EmitSignal(SignalName.UpdateInventory);
	}
	
	public void PlayerInputReceived(String textInput)
	{
		// Echo input in output window
		OutputText(">" + textInput + "\n");
		
		// Execute command
		CommandResult result = Player.Instance.ExecuteCommand(textInput.ToLower());
		
		// Output text
		OutputText(result.Text + "\n");
		
		if (result.Command == MoveCommand.Instance && result.Success == true)
		{
			// Update room grid on minimap
			var visitedStatus = World.Instance.GetVisitedStatusForAllRooms();
			var godotDict = new Godot.Collections.Dictionary<string, bool>();

			foreach (var kvp in visitedStatus)
			{
				godotDict[kvp.Key] = kvp.Value;
			}
			EmitSignal(SignalName.MapMove, Player.Instance.CurrentRoom.Name, godotDict);
			
			// Play walking sound
			AudioManager.Instance.PlaySFX("walk");
		}
		
		// Update arrows on minimap
		EmitSignal(SignalName.ArrowUpdate);
		
		// Output hint when player enters three consecutive invalid commands
		if (result.Command != InvalidCommand.Instance)
		{
			invalidCommandCount = 0;
		}
		else if (++invalidCommandCount > 2)
		{
			OutputText("Type 'help' for a description of available commands.\n");
		}
		
		//
		// Use specific happenings
		//
		
		// Handle puzzles
		GeneratorPuzzle(result);
		StasisPuzzle(result);
		
		if (result.UseAction == openDoor)
		{
			AudioManager.Instance.PlaySFX("door_open");
		}
		
		if (result.UseAction == openStorageBox)
		{
			AudioManager.Instance.PlaySFX("item_multitool");
		}
		
		if (result.UseAction == removeRubble)
		{
			AudioManager.Instance.PlaySFX("pickup_0");
		}
		
		// Pickup sounds (track, startTime, endTime)
		if (result.Command == TakeCommand.Instance && result.ItemsObtained.Contains(redRods))
		{
			AudioManager.Instance.PlaySFX("pickup_3",0f, 0.8f);
		}
		if (result.Command == TakeCommand.Instance && result.ItemsObtained.Contains(greenRods))
		{
			AudioManager.Instance.PlaySFX("pickup_3",0f, 0.8f);
		}
		if (result.Command == TakeCommand.Instance && result.ItemsObtained.Contains(blueRods))
		{
			AudioManager.Instance.PlaySFX("pickup_3",0f, 0.8f);
		}
		if (result.Command == UseCommand.Instance && result.UseAction.RequiredItems[0].Name.Contains("Rods"))
		{
			AudioManager.Instance.PlaySFX("event_keycard_0",0f, 0.7f);
		}
		if (result.Command == UseCommand.Instance && result.UseAction.RequiredItems[0].Name.Contains("Cell"))
		{
			AudioManager.Instance.PlaySFX("pickup_0",0f, 0.4f);
		}
		if (result.UseAction != null && result.UseAction.RequiredItems.Count == 1
			&& result.UseAction.RequiredItems[0].Name.Contains("Pod"))
		{
			AudioManager.Instance.PlaySFX("event_unlock_0",0.9f);
		}
		
		// Room ambience swaps
		if (result.Command == MoveCommand.Instance && Player.Instance.CurrentRoom.Name == "Heart Chamber" && World.Instance.IsPowerOn)
		{
			AudioManager.Instance.PlaybgAmbience("loop_0");
		}
		if (result.Command == MoveCommand.Instance && Player.Instance.CurrentRoom.Name == "Elevator Shaft")
		{
			AudioManager.Instance.PlaybgAmbience("loop_1");
		}
		if (result.Command == MoveCommand.Instance && Player.Instance.CurrentRoom.Name == "Cramped Hallway")
		{
			AudioManager.Instance.PlaybgAmbience("loop_4");
		}
		
		
		// Update inventory screen 
		EmitSignal(SignalName.UpdateInventory);
	}
}
