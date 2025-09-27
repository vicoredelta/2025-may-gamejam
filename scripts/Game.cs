using Godot;
using System;
using System.Collections.Generic;
using Godot.Collections;

public partial class Game : Node
{
	ItemType console;
	ItemType redCable;
	ItemType blueCable;
	ItemType greenCable;
	UseAction attachPowerCell;
	UseAction openDoor;
	UseAction openStorageBox;
	UseAction removeRubble;
	UseAction useRedCable;
	UseAction useBlueCable;
	UseAction useGreenCable;
	int invalidCommandCount = 0;
	int redCableUseCount = 0;
	int blueCableUseCount = 0;
	int greenCableUseCount = 0;
	
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
		"You see the [color=efad42]mummified remains[/color] of an unknown person, their bones and shriveled skin mixed with debris. " +
		"Was this person part of the crew, or a looter?",
		false);
		
		World.Instance.CreateItemType("Self", [], // Should be in every room
		"You take a moment to feel yourself wasting away. Better get moving.",
		false, false);
		
		World.Instance.CreateItemType("Ladder", [], // Room: Elevator Shaft
		"You spot bolt holes in the wall where a ladder should be.",
		false, false);
		
		World.Instance.CreateItemType("Bolt Holes", [], // Room: Elevator Shaft
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
		
		World.Instance.CreateItemType("Storage", [], // Room: Heart Chamber
		"A storage box, with a simple [color=7b84ff]electronic lock[/color]. " +
		"The box is made of some heat resistant, lightweight metal. We still haven't been able to replicate anything like it.",
		false);
		redCable = World.Instance.CreateItemType("Red Cable", [], "It's a red cable.", true);
		blueCable = World.Instance.CreateItemType("Blue Cable", [], "It's a blue cable.", true);
		greenCable = World.Instance.CreateItemType("Green Cable", [], "It's a green cable.", true);
		
		// Define uses (required items, produced items, destroyed items, create location, description, requires power on [optional], requires power cell [optional])
		removeRubble = World.Instance.CreateUse(
			["Rubble"], ["Storage"], ["Rubble"], ItemCreateLocation.Room,
			"With little effort the rubble is cleared, revealing a [color=7b84ff]storage box[/color] with a simple electronic lock."
		);
		
		attachPowerCell = World.Instance.CreateUse(
			["Stolen Power Cell", "Console"], [], ["Stolen Power Cell"], ItemCreateLocation.Room,
			"You place the power cell into the hatch and attach the cables."
		);
		
		openStorageBox = World.Instance.CreateUse(
			["Wracker", "Storage"], ["Red Cable", "Blue Cable", "Green Cable"], ["Storage"], ItemCreateLocation.Room,
			"With a click and a chime the lock is undone and the box lid opens to reveal a large assortment of coloured [color=38a868]cables[/color]. " + 
			"The box contains green, red, and blue cables."
		);
		
		useRedCable = World.Instance.CreateUse(
			["Red Cable", "Console"], [], [], ItemCreateLocation.Room,
			"You use the red cable to supply some power to the ship.", requiresCell: true
		);
		
		useBlueCable = World.Instance.CreateUse(
			["Blue Cable", "Console"], [], [], ItemCreateLocation.Room,
			"You use the blue cable to supply some power to the ship.", requiresCell: true
		);
		
		useGreenCable = World.Instance.CreateUse(
			["Green Cable", "Console"], [], [], ItemCreateLocation.Room,
			"You use the green cable to supply some power to the ship.", requiresCell: true
		);
		
		openDoor = World.Instance.CreateUse(
			["Door"], [], ["Door"], ItemCreateLocation.Room,
			"Although you're somewhat drained from the journey through the wasteland, you still manage to pry the door open. Phew!"
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
		
		if (result.UseAction == attachPowerCell)
		{
			World.Instance.CellIsPlaced = true;
			//console.Description = "Some kind of console. You hear the hum of its fan working beneath the casing.";
		}
		
		// Generator puzzle
		if (result.UseAction == useBlueCable || result.UseAction == useRedCable || result.UseAction == useGreenCable)
		{
			if (result.UseAction == useBlueCable)
			{
				if (blueCableUseCount++ < 2)
				{
					World.Instance.ShipPower += 20;
					OutputText("Ship power is increased by 20 units. Current level is " + World.Instance.ShipPower+ ".");
				}
				else
				{
					OutputText("This cable currently won't provide more power.");
				}
			}
			else if (result.UseAction == useRedCable)
			{
				if (redCableUseCount++ < 2)
				{
					World.Instance.ShipPower += 30;
					OutputText("Ship power is increased by 30 units. Current level is " + World.Instance.ShipPower+ ".");
				}
				else
				{
					OutputText("This cable currently won't provide more power.");
				}
			}
			else if (result.UseAction == useGreenCable)
			{
				if (greenCableUseCount++ < 2)
				{
					World.Instance.ShipPower += 15;
					OutputText("Ship power is increased by 15 units. Current level is " + World.Instance.ShipPower+ ".");
				}
				else
				{
					OutputText("This cable currently won't provide more power.");
				}
			}
			
			if (World.Instance.ShipPower > 100)
			{
				World.Instance.ShipPower = 0;
				OutputText("As power increases to over 100 units the system short circuits and power level is reset to 0.");
				greenCableUseCount = 0;
				redCableUseCount = 0;
				blueCableUseCount = 0;
			}
			else if (World.Instance.ShipPower == 100)
			{
				World.Instance.IsPowerOn = true;
				AudioManager.Instance.PlaySFX("event_powercell");
				OutputText("As power level reaches exactly 100 units power to the ship is completely restored.");
				Player.Instance.Take(redCable);
				Player.Instance.Take(blueCable);
				Player.Instance.Take(greenCable);
				Player.Instance.CurrentRoom.Take(redCable);
				Player.Instance.CurrentRoom.Take(blueCable);
				Player.Instance.CurrentRoom.Take(greenCable);
			}
			
			OutputText("\n");
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
		
		// Example to do something on specific pick up
		if (result.Command == TakeCommand.Instance && result.ItemsObtained.Contains(redCable))
		{
			// Do thing
		}
		
		// Update inventory screen 
		EmitSignal(SignalName.UpdateInventory);
	}
}
