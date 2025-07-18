using Godot;
using System;
using System.Collections.Generic;

public partial class Player
{
	public CommandOutput ExecuteCommand(CommandInput input)
	{
		switch (input.Command)
		{
		case Command.Use:
			if (input.Items.Count == 0)
			{
				return new CommandOutput("You must specify one or several items.");
			}
			
			ItemUse foundUse = FindUse(input.Items);
		
			if (foundUse != null)
			{
				return foundUse.Use(_inventory, _currentRoom);
			}
			
			return new CommandOutput();
			
		case Command.Look:
			if (input.Items.Count == 0 ||
				!(_inventory.HasItem(input.Items[0]) || _currentRoom.HasItem(input.Items[0])))
			{
				String text = _currentRoom.Description;
				
				if (_currentRoom.ListItems() != "")
				{
					text = text + "\n" + _currentRoom.ListItems();
				}
				
				return new CommandOutput(Command.Look, text);
			}
			else
			{
				return new CommandOutput(Command.Look, input.Items[0].Description);
			}
			
		case Command.Move:
			if (input.Direction == Direction.InvalidDirection)
				return new CommandOutput("You must specify a direction.");
			
			Room connectingRoom = _currentRoom.GetConnectingRoom(input.Direction);
			
			if (connectingRoom != null)
			{
				_currentRoom = connectingRoom;
				return new CommandOutput(input.Direction, "You move " + input.Direction.ToString().ToLower() + ".");
			}
			
			return new CommandOutput("There is nowhere to go " + input.Direction.ToString().ToLower() + ".");
			
		case Command.Help:
			return new CommandOutput(Command.Help,
			"Type [color=de6ba5][look][/color] for a description of your current " +
			"surroundings, \n[color=de6ba5][take][/color] to pick up an item, \n[color=de6ba5][move][/color] to " +
			"walk to a different room, " +
			"or [color=de6ba5][use][/color] to use an item.\n" +
			"[color=de6ba5][look][/color] may be followed by an item in the vicinity or in your inventory " +
			"in order to take a closer look at it.\n" +
			"[color=de6ba5][move][/color] must be followed by a direction, such as [color=7b84ff][north][/color].\n" +
			"[color=de6ba5][take][/color] must be followed by an item in the vicinity, such as [color=38a868][key][/color].\n" +
			"[color=de6ba5][use][/color] must be followed by one or more items.");
			
		case Command.Take:
			if (input.Items.Count == 0 || !_currentRoom.HasItem(input.Items[0]))
			{
				return new CommandOutput("You must specify an item in the room.");
			}
			else if (!input.Items[0].CanBePickedUp)
			{
				return new CommandOutput("You can not pick up the " + input.Items[0].Name.ToLower() + ".");
			}
			else
			{
				_currentRoom.TakeItem(input.Items[0]);
				return new CommandOutput("You pick up the " + input.Items[0].Name.ToLower() + ".", input.Items[0]);
			}
			
		default:
			return new CommandOutput("Invalid command.");
		}
	}
}
