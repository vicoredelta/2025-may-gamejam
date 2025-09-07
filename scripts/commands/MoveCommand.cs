using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class MoveCommand : Command
{
	// Make singleton
	private MoveCommand() { }
	public static MoveCommand Instance { get; private set; } = new MoveCommand();
	
	public override CommandResult Execute(String[] words, Player player, Room currentRoom)
	{
		Direction direction = Parser.GetDirection(words);
		
		// Output error message if no direction was given
		if (direction == Direction.InvalidDirection)
		{
			return new CommandResult("You must specify a [color=7b84ff]direction[/color].");
		}
		
		Room connectingRoom = currentRoom.GetConnectingRoom(direction);
		
		if (connectingRoom != null)
		{
			if (currentRoom.ObstaclesExist(direction))
			{
				// Output list of obstacles if there are any blocking the way
				return new CommandResult(currentRoom.ListObstacles(direction));
			}
			
			player.CurrentRoom = connectingRoom;
			String outText = "You move " + direction.ToString().ToLower() + ".";
			
			if (!currentRoom.Visited)
			{
				outText += " " + currentRoom.FirstTimeDescription;
				currentRoom.Visited = true;
			}
			
			return new CommandResult(direction, outText);
		}
		else
		{
			// Output message if there was no room in the specifed direction
			return new CommandResult("There is nowhere to go " + direction.ToString().ToLower() + ".");
		}
	}
}
