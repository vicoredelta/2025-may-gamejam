using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class MoveCommand : Command
{
	// Make singleton
	private MoveCommand() { }
	public static MoveCommand Instance { get; private set; } = new MoveCommand();
	
	public override CommandResult Execute(String[] words)
	{
		// Skip first word
		words = words.Skip(1).ToArray();
		
		Direction direction = Parser.GetDirection(words);
		
		// Output error message if no direction was given
		if (direction == Direction.InvalidDirection)
		{
			return new CommandResult(this, "You must specify a [color=7b84ff]direction[/color].");
		}
		
		Room connectingRoom = Player.Instance.CurrentRoom.GetConnectingRoom(direction);
		
		if (connectingRoom != null)
		{
			if (Player.Instance.CurrentRoom.ObstaclesExist(direction))
			{
				// Output list of obstacles if there are any blocking the way
				return new CommandResult(this, Player.Instance.CurrentRoom.ListObstacles(direction));
			}
			
			String outText = "You move " + direction.ToString().ToLower() + ".";
			
			if (!connectingRoom.Visited)
			{
				outText += " " + Player.Instance.CurrentRoom.FirstTimeDescription;
			}
			
			connectingRoom.Visited = true;
			Player.Instance.CurrentRoom = connectingRoom;
			return new CommandResult(direction, outText);
		}
		else
		{
			// Output message if there was no room in the specifed direction
			return new CommandResult(this, "There is nowhere to go " + direction.ToString().ToLower() + ".");
		}
	}
}
