using Godot;
using System;
using System.Collections.Generic;

public interface IExecutable
{
	public CommandResult Execute(String[] words, Player player, Room currentRoom);
}

public abstract class Command : IExecutable
{
	public abstract CommandResult Execute(String[] words, Player player, Room currentRoom);
}
