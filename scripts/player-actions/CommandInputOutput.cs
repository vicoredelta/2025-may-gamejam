using Godot;
using System;
using System.Collections.Generic;

public class CommandInput
{
	public Command Command { get; }
	public List<ItemType> Items { get; } = new List<ItemType>();
	public Direction Direction { get; } = Direction.InvalidDirection;
	
	public CommandInput(Command command, List<ItemType> items, Direction direction)
	{
		Command = command;
		Direction = direction;
		Items.AddRange(items);
	}
}

public class CommandOutput
{
	public String Text { get; } = "Nothing interesting happens.";
	public Command Command { get; } = Command.InvalidCommand;
	public List<ItemType> ItemsObtained { get; } = new List<ItemType>();
	public List<ItemType> ItemsLost { get; } = new List<ItemType>();
	public Direction Direction { get; } = Direction.InvalidDirection;
	
	public CommandOutput() { }
	
	public CommandOutput(Command command, String text)
	{
		Command = command;
		Text = text;
	}
	
	public CommandOutput(Direction direction, String text)
	{
		Command = Command.Move;
		Direction = direction;
		Text = text;
	}
	
	public CommandOutput(String text)
	{
		Text = text;
	}
	
	public CommandOutput(String text, ItemType itemType)
	{
		Command = Command.Take;
		Text = text;
		ItemsObtained.Add(itemType);
	}
	
	public CommandOutput(String text, List<ItemType> itemsObtained, List<ItemType> itemsLost)
	{
		Command = Command.Use;
		Text = text;
		ItemsObtained.AddRange(itemsObtained);
		ItemsLost.AddRange(itemsObtained);
	}
}
