using Godot;
using System;

// Represents a connection between two adjacent rooms
public class Connection : ItemHolder
{
	Room Room1 { get; }
	Room Room2 { get; }
	Direction Direction { get; }	// Direction from Room1 to Room2
	
	public Connection(Room room1, Room room2, Direction direction)
	{
		Room1 = room1;
		Room2 = room2;
		Direction = direction;
	}
	
	public Room GetDestination(Room originRoom)
	{
		if (Room1 == originRoom)
		{
			return Room2;
		}
		else if (Room2 == originRoom)
		{
			return Room1;
		}
		else
		{
			return null;
		}
	}
}
