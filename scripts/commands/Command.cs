using Godot;
using System;
using System.Collections.Generic;

public interface IExecutable
{
	public String[] Aliases { get; }
	public CommandResult Execute(String[] words, Player player, Room currentRoom);
}

public abstract class CommandX : IExecutable
{
	public abstract String[] Aliases { get; }
	public abstract CommandResult Execute(String[] words, Player player, Room currentRoom);
}
