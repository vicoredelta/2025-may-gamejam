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
