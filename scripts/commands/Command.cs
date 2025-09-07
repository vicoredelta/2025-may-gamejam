using Godot;
using System;
using System.Collections.Generic;

public interface IExecutable
{
	public CommandResult Execute(String[] words);
}

public abstract class Command : IExecutable
{
	public abstract CommandResult Execute(String[] words);
}
