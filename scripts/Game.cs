using Godot;
using System;
using System.Collections.Generic;
using Godot.Collections;

public partial class Game : Node
{
	ItemType console;
	UseAction attachPowerCell;
	UseAction openDoor;
	UseAction openStorageBox;
	UseAction removeRubble;
	
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
		
		// Create rooms (name, descripion, first time description [optional]), starting room is created in World.cs
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
		"so preserving food might've been to advanced even for the ancients. What a peculiar culture. " +
		"There is only one door, to the [color=7b84ff]north[/color].",
		"You walk into a [color=efad42]small alcove[/color]. There's a stove and a number of broken kitchenware scattered around the room."
		);
		
		// Set starting room
		World.Instance.SetCurrentRoom("Breached Entrance");
		
		// Define connections between rooms (room 1, room 2, direction when moving from room 1 to room 2)
		World.Instance.ConnectRooms("Breached Entrance", "Cramped Hallway", Direction.North);
		World.Instance.ConnectRooms("Cramped Hallway", "Heart Chamber", Direction.North);
		World.Instance.ConnectRooms("Heart Chamber", "Captain's Quarters", Direction.North);
		World.Instance.ConnectRooms("Heart Chamber", "Elevator Shaft", Direction.West);
		World.Instance.ConnectRooms("Heart Chamber", "Strange Panels", Direction.East);
		World.Instance.ConnectRooms("Strange Panels", "Kitchen Alcove", Direction.South);
		
		// Define every unique type of item (item name, item description, can be picked up, is visible [optional], icon path [optional])
		
		World.Instance.CreateItemType("Wracker", // Starting inventory
		"It's a [color=38a868]wracker[/color], a transforming multitool. You've rigged it for bypassing " +
		"older, low-leveled passwords and locks. It's almost brand new, " + 
		"but the attached gemstone has been with your family for generations.",
		true, "res://assets/item_multitool_0.png");
		
		World.Instance.CreateItemType("Stolen Power Cell", // Starting inventory
		"An outmode, clockwork [color=38a868]generator[/color]. A low, " +
		"hurried ticking and a faint glow suggest that it's still functional.",
		true, "res://assets/item_powercell_0.png");
		
		World.Instance.CreateItemType("Carcass", // Room: Breached Entrance
		"The remains of a small animal, possibly a rodent. It might have sought shelter from the sweltering heat, " +
		"but was unable to claw its way out again.",
		false);
		
		World.Instance.CreateItemType("Door", // Room: Breached Entrance
		"A mangled, metal [color=7b84ff]door[/color] blocks your way forward.",
		false);
		
		World.Instance.CreateItemType("Outside", // Room: Breached Entrance
		"You can see the outside from the hole you entered through. Ripples of heat shimmer cover the red and yellow landscape.",
		false, false);
		
		World.Instance.CreateItemType("Wasteland", // Room: Breached Entrance
		"You can see the outside from the hole you entered through. Ripples of heat shimmer cover the red and yellow landscape.",
		false, false);
		
		World.Instance.CreateItemType("Debris", // Room: Cramped Hallway
		"There's a large, knife-edged piece of metallic wreckage in the middle of the hallway. " +
		"Part of the floor must have ruptured in the crash. You'd better stay clear of it.",
		false);
		
		World.Instance.CreateItemType("Wall", // Room: Cramped Hallway
		"The walls are covered with metre-thick, cylindrical padding.",
		false, false);
		
		World.Instance.CreateItemType("Rubble", // Room: Heart Chamber
		"A mound of splintered blackstone and smashed machinery. Some kind of [color=7b84ff]storage box[/color] " + 
		"lies half-buried under the mess.",
		false);
		
		console = World.Instance.CreateItemType("Console", // Room: Heart Chamber
		"An antique control panel. The panels around the [color=7b84ff]console[/color] are covered with primitive buttons, " +
		"switches, and sockets for who knows what purpose. One socket in particular catches your eye; " +
		"a [color=38a868]power cell[/color] might fit.",
		false);
		
		World.Instance.CreateItemType("Corpse", // Room: Elevator Shaft
		"The corpse of a looter, presumably. You can't make out any distinct features, but it appears to be an adult. " +
		"Judging by the stench, they likely died about a week ago.",
		false);
		
		World.Instance.CreateItemType("Mummified Corpse", // Room: Kitchen Alcove
		"You see the [color=efad42]mummified remains[/color] of an unknown person, their bones and shriveled skin mixed with debris. " +
		"Was this person part of the crew, or a looter?",
		false);
		
		World.Instance.CreateItemType("Self", // Should be in every room
		"You take a moment to feel yourself wasting away. Better get moving.",
		false, false);
		
		World.Instance.CreateItemType("Ladder", // Room: Elevator Shaft
		"You spot bolt holes in the wall where a ladder should be.",
		false, false);
		
		World.Instance.CreateItemType("Body", // Room: Elevator Shaft
		"The corpse of a looter, presumably. You can't make out any distinct features, but it appears to be an adult. " +
		"Judging by the stench, they likely died about a week ago.",
		false, false);
		
		World.Instance.CreateItemType("Code Lock", // Room: Heart Chamber
		"A metal door with a keypad terminal is blocking the way east.",
		false);
		
		World.Instance.CreateItemType("Note", // Room: Captain's Quarters
		"A small note with the letters '[color=7b84ff]CXXIII[/color]' written on it.",
		false);
		
		World.Instance.CreateItemType("Storage", // Room: Heart Chamber
		"A storage box, with a simple [color=7b84ff]electronic lock[/color]. " +
		"The box is made of some heat resistant, lightweight metal. We still haven't been able to replicate anything like it.",
		false);
		World.Instance.CreateItemType("Red Cable", "It's a red cable.", true);
		World.Instance.CreateItemType("Blue Cable", "It's a blue cable.", true);
		World.Instance.CreateItemType("Green Cable", "It's a green cable.", true);
		World.Instance.CreateItemType("Purple Cable", "It's a purple cable.", true);
		
		// Define uses (required items, produced items, destroyed items, create location, requires power, description)
		removeRubble = World.Instance.CreateUse(
			["Rubble"], ["Storage"], ["Rubble"], ItemCreateLocation.Room,false,
			"With little effort the rubble is cleared, revealing a [color=7b84ff]storage box[/color] with a simple electronic lock."
		);
		
		attachPowerCell = World.Instance.CreateUse(
			["Stolen Power Cell", "Console"], [], ["Stolen Power Cell"], ItemCreateLocation.Room, false,
			"You place the power cell into the hatch and attach the cables. There is a small hiss as the ships power returns."
		);
		
		openStorageBox = World.Instance.CreateUse(
			["Wracker", "Storage"], ["Red Cable", "Blue Cable", "Green Cable", "Purple Cable"], ["Storage"], ItemCreateLocation.Room, false,
			"With a click and a chime the lock is undone and the box lid opens to reveal a large assortment of coloured [color=38a868]cables[/color]. " + 
			"The box contains green, purple, red, and blue cables."
		);
		
		openDoor = World.Instance.CreateUse(
			["Door"], [], ["Door"], ItemCreateLocation.Room, false,
			"Although you're somewhat drained from the journey through the wasteland, you still manage to pry the door open. Phew!"
		);
		
		// Define input actions (required item, producedItems, create location, description on correct input, description on wrong input, required input)
		World.Instance.CreateInputAction(
			"Code Lock", [], ItemCreateLocation.Room, true,
			"The terminal beeps approvingly. The door unlocks!", "The terminal beeps in disapprovement. The door remains locked.",
			"123"
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
		World.Instance.AddItemToRoom("Mummified Corpse", "Kitchen Alcove");
		
		// 'Self' is a hidden item, added to every room individually
		World.Instance.AddItemToRoom("Self", "Breached Entrance");
		World.Instance.AddItemToRoom("Self", "Cramped Hallway");
		World.Instance.AddItemToRoom("Self", "Heart Chamber");
		World.Instance.AddItemToRoom("Self", "Elevator Shaft");
		World.Instance.AddItemToRoom("Self", "Captain's Quarters");
		World.Instance.AddItemToRoom("Self", "Strange Panels");
		World.Instance.AddItemToRoom("Self", "Kitchen Alcove");
		
		// Add items as obstacles between rooms (item, room, direction to block)
		World.Instance.AddItemAsObstacle("Door", "Breached Entrance", Direction.North);
		World.Instance.AddItemAsObstacle("Code Lock", "Heart Chamber", Direction.East);
		
		// Add items to player inventory
		AddStartingItems(["Wracker", "Stolen Power Cell"]);
		
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
			EmitSignal(SignalName.ModifyInventory, item, World.Instance.GetItem(item).IconPath, true);
		}
	}
	
	public void PlayerInputReceived(String textInput)
	{
		// Echo input in output window
		OutputText(">" + textInput + "\n");
		
		// Execute command
		CommandResult result = Player.Instance.ExecuteCommand(textInput.ToLower());
		
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
		
		if (result.Command == MoveCommand.Instance)
		{
			// Update minimap
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
		
		// Use specific happenings
		if (result.UseAction == attachPowerCell)
		{
			World.Instance.IsPowerOn = true;
			console.Description = "Some kind of console. You hear the humm of its fan working beneath the casing.";
			AudioManager.Instance.PlaySFX("event_powercell");
		}
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
	}
}
