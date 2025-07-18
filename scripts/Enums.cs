using Godot;
using System;

public enum Direction
{
	North,
	West,
	East,
	South,
	InvalidDirection
}

public enum ItemCreateLocation
{
	Player,
	Room
}

public enum Command
{
	Use,
	Look,
	Move,
	Take,
	Help,
	InvalidCommand
}
