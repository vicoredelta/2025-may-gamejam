using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

/*
public class MoveCommand : CommandX
{
	// Make singleton
	private MoveCommand() { }
	public static MoveCommand Instance { get; private set; } = new MoveCommand();
	
	public override CommandResult Execute(String[] words, Player player, Room currentRoom)
	{
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
	}
}
*/
