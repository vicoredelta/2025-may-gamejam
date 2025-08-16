using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Player
{
	public CommandResult ExecuteCommand(CommandInput input)
	{
		switch (input.Command)
		{
		case Command.Use:
			if (input.Items.Count == 0)
			{
				return new CommandResult("You must specify one, or several, [color=7b84ff]items[/color].");
			}
			
			ItemUse foundUse = FindUse(input.Items);
		
			if (foundUse != null)
			{
				if (foundUse.RequiresPower && !_world.IsPowerOn)
				{
					return new CommandResult("Nothing happens. Maybe it needs power?");
				}
				return foundUse.Use(this, _currentRoom);
			}
			
			return new CommandResult();
			
		case Command.Look:
			if (input.Items.Count == 0 ||
				!(this.HasItem(input.Items[0]) || _currentRoom.HasItem(input.Items[0])))
			{
				String text = _currentRoom.Description;
				
				if (_currentRoom.ListItems() != "")
				{
					text = text + "\n" + _currentRoom.ListItems();
				}
				
				return new CommandResult(Command.Look, text);
			}
			else
			{
				return new CommandResult(Command.Look, input.Items[0].Description);
			}
		case Command.Move:
			if (input.Direction == Direction.InvalidDirection)
				return new CommandResult("You must specify a [color=7b84ff]direction[/color].");
			
			Room connectingRoom = _currentRoom.GetConnectingRoom(input.Direction);
			
			if (connectingRoom != null)
			{
				if (_currentRoom.ObstaclesExist(input.Direction))
				{
					return new CommandResult(_currentRoom.ListObstacles(input.Direction));
				}
				
				_currentRoom = connectingRoom;
				String outText = "You move " + input.Direction.ToString().ToLower() + ".";
				
				if (!_currentRoom.Visited)
				{
					outText += " " + _currentRoom.FirstTimeDescription;
					_currentRoom.Visited = true;
				}
				
				return new CommandResult(input.Direction, outText);
			}
			
			return new CommandResult("There is nowhere to go " + input.Direction.ToString().ToLower() + ".");
			
		// The 'help' command is typed to explain basic controls to the player.
		case Command.Help:
			return new CommandResult(Command.Help,
			"[color=efad42]===Movement commands===[/color]" +
			"\nThe [color=de6ba5]Move[/color] command must be followed by a direction, such as [color=7b84ff]north[/color]. " +
			"You may also type [color=de6ba5]go[/color] or [color=7b84ff]up[/color]." +
			"\n[color=efad42]===Investigation commands===[/color]" +
			"\nType [color=de6ba5]look[/color] for a description of your current " +
			"surroundings. [color=de6ba5]Look[/color] may be followed by an [color=7b84ff]item[/color] in the vicinity " +
			"or in your [color=38a868]inventory[/color]." +
			"\nYou can interact with doors, items, and the environment, by typing [color=de6ba5]use[/color], " +
			"followed by one or more items. For example: [color=de6ba5]use[/color] " +
			"[color=7b84ff]door[/color], [color=de6ba5]use[/color] [color=38a868]key[/color] [color=7b84ff]safebox[/color].\n" +
			" The same rules are true for the [color=de6ba5]Take[/color] command. " +
			"For example: [color=de6ba5]take[/color] [color=38a868]key[/color]. " +
			"Items that may be picked up are usually highlighted in [color=38a868]green[/color]." +
			"\n[color=efad42]===Input commands===[/color]" +
			"\nSome items can be interacted with by inputing text, like a password. To perform this, you must type " +
			"[color=de6ba5]input[/color], followed by the [color=7b84ff]item[/color] (like a keypad), and " +
			"finally closed with the [color=7b84ff]input[/color] text. " +
			"For example: [color=de6ba5]input[/color] [color=7b84ff]keypad[/color] [color=7b84ff]12345[/color].\n"
			);
			
		// The 'credits' command is typed to display the game's title and authors.	
		case Command.Credits:
			return new CommandResult(Command.Credits,
			"[color=efad42]Raid on the Sarcophagus Engine[/color]" + "\n [color=7b84ff]Programming:[/color] Emil Åberg & Gillis Gröndahl" +
			"\n [color=7b84ff]Design & Graphics:[/color] Christoffer Eriksson & Isac Berg" + 
			"\n [color=7b84ff]Text:[/color] David Sundqvist & Isac Berg" +
			"\n [color=7b84ff]Audio:[/color] David Sundqvist" +
			"\n [color=7b84ff]Game by:[/color] Chen Space Program" +
			"\n Thank you for playing!"
			);
			
		case Command.Take:
			if (input.Items.Count == 0 || !_currentRoom.HasItem(input.Items[0]))
			{
				return new CommandResult("You must specify an [color=7b84ff]item[/color] in the room.");
			}
			else if (!input.Items[0].CanBePickedUp)
			{
				return new CommandResult("You can not pick up the " + input.Items[0].Name.ToLower() + ".");
			}
			else
			{
				_currentRoom.Take(input.Items[0]);
				return new CommandResult("You pick up the " + input.Items[0].Name.ToLower() + ".", input.Items[0]);
			}
			
		case Command.Input:
			if (input.Items.Count == 0)
			{
				return new CommandResult("You must specify an [color=7b84ff]item[/color].");
			}
			else if (!this.HasItem(input.Items[0]) && !_currentRoom.HasItem(input.Items[0]))
			{
				return new CommandResult("There is no " + input.Items[0] + " in the vicinity");
			}
			else if (input.EntryText == "")
			{
				return new CommandResult("You need write some [color=de6ba5]input[/color] for " + input.Items[0] + ".");
			}
			else
			{
				InputAction action = FindInputAction(input.Items[0]);
				
				if (action == null)
				{
					return new CommandResult();
				}
				else
				{
					return action.Activate(this, _currentRoom, input.EntryText);
				}
			}
			
		default:
			return new CommandResult("Invalid command.");
		}
	}
}
