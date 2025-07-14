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
			String text = _currentRoom.Description;
			
			if (_currentRoom.ListItems() != "")
			{
				text = text + "\n" + _currentRoom.ListItems();
			}
			
			return new CommandOutput(Command.Look, text);
			
		case Command.Examine:
			if (input.Items.Count == 0)
				return new CommandOutput("You must specify an in the vicinity or on your person.");
			else
				return new CommandOutput(Command.Examine, input.Items[0].Description);
			
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
			"Type [look] for a description of an item or your current " +
			"surroundings, [take] to pick up an item, [move] to " +
			"walk to a different room, [examine] to look closer at item, " +
			"or [use] to use an item.\n" +
			"[move] must be followed by a direction, such as [north] or [left]. " +
			"[take] must be followed by an item in the vicinity, such as [key] or [gadget]. " +
			"[use] must be followed by one or several items.");
			
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
