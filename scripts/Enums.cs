using Godot;
using System;

// Public enums that are used for player movement. Called by 'Parser.cs'
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

// Public enums that are used for player interaction. Called by 'Parser.cs'
public enum Command
{
	Credits,
	Help,
	Input,
	Listen,
	Look,
	Move,
	Take,
	Use,
	
	InvalidCommand
}
