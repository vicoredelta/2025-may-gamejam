using Godot;
using System;
using System.Collections.Generic;

public class Parser
{
	public Parser()
	{
		
	}
	
	public (Command, List<ItemType>, Direction) Read(String text)
	{
		// TODO: finish this
		
		return (Command.Use, new List<ItemType>(), Direction.North);
	}
}
