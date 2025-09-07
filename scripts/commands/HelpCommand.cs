using Godot;
using System;
using System.Collections.Generic;

public class HelpCommand : Command
{
	// Make singleton
	private HelpCommand() { }
	public static HelpCommand Instance { get; private set; } = new HelpCommand();
	
	public override CommandResult Execute(String[] words, Player player, Room currentRoom)
	{
		return new CommandResult(HelpCommand.Instance,
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
	}
}
