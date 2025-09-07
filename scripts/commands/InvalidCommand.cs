using Godot;
using System;
using System.Collections.Generic;

public class InvalidCommand : Command
{
	// Make singleton
	private InvalidCommand() { }
	public static InvalidCommand Instance { get; private set; } = new InvalidCommand();
	
	public override CommandResult Execute(String[] words, Player player, Room currentRoom)
	{
		return new CommandResult();
	}
}
