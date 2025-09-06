using Godot;
using System;
using System.Collections.Generic;

public class CreditsCommand : CommandX
{
	// Make singleton
	private CreditsCommand() { }
	public static CreditsCommand Instance { get; private set; } = new CreditsCommand();
	
	public override CommandResult Execute(String[] words, Player player, Room currentRoom)
	{
		return new CommandResult(Command.Credits,
			"[color=efad42]Raid on the Sarcophagus Engine[/color]" + "\n [color=7b84ff]Programming:[/color] Emil Åberg & Gillis Gröndahl" +
			"\n [color=7b84ff]Design & Graphics:[/color] Christoffer Eriksson & Isac Berg" + 
			"\n [color=7b84ff]Text:[/color] David Sundqvist & Isac Berg" +
			"\n [color=7b84ff]Audio:[/color] David Sundqvist" +
			"\n [color=7b84ff]Game by:[/color] Chen Space Program" +
			"\n Thank you for playing!"
			);
	}
}
