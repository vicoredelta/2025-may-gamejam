using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class MoveCommand : CommandX
{
	// Make singleton
	private MoveCommand() { }
	public static MoveCommand Instance { get; private set; } = new MoveCommand();
	
	public override CommandResult Execute(Player player, Room currentRoom)
	{
		return new CommandResult();
	}
	
	public override void ParseInput(String[] words, Player player, Room currentRoom)
	{
		
	}
}
