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
			ItemUse foundUse = FindUse(input.Items);
		
			if (foundUse != null)
			{
				return foundUse.Use(_inventory, _currentRoom);
			}
			else
			{
				return new CommandOutput();
			}
			
		case Command.Look:
			String text = _currentRoom.Description;
			
			if (_currentRoom.ListItems() != "")
			{
				text = text + _currentRoom.ListItems() + "\n";
			}
			
			return new CommandOutput(Command.Look, text);
			
		case Command.Examine:
			if (input.Items.Count == 0)
				return new CommandOutput();
			else
				return new CommandOutput(Command.Examine, input.Items[0].Description);
			
		case Command.Move:
			if (input.Direction == Direction.InvalidDirection) return new CommandOutput();
			Room connectingRoom = _currentRoom.GetConnectingRoom(input.Direction);
			
			if (connectingRoom != null)
			{
				_currentRoom = connectingRoom;
				return new CommandOutput(input.Direction, "You move " + input.Direction.ToString().ToLower());
			}
			else
			{
				return new CommandOutput("There is nowhere to go " + input.Direction.ToString().ToLower() + ".");
			}
			
		case Command.Help:
			return new CommandOutput(Command.Help,
			"Type [look] or [examine] for a description of an item or your " +
			"current surroundings.\n[walk] or [move] must be followed by a " +
			"direction, such as [north] or [left].\n[take] or [grab] must be " +
			"followed by a noun, such as [key] or [gadget].");
			
		case Command.Take:
			if (input.Items.Count == 0 || !_currentRoom.HasItem(input.Items[0]))
			{
				return new CommandOutput();
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
			return new CommandOutput();
		}
	}
}
