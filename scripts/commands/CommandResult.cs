using Godot;
using System;
using System.Collections.Generic;

// Struct to hold information about an executed command
public struct CommandResult
{
	public String Text { get; } = "Nothing interesting happens.";
	public IExecutable Command { get; } = InvalidCommand.Instance;
	public List<ItemType> ItemsObtained { get; } = new List<ItemType>();
	public List<ItemType> ItemsLost { get; } = new List<ItemType>();
	public Direction Direction { get; } = Direction.InvalidDirection;
	public UseAction UseAction { get; } = null;
	
	public CommandResult() { }
	
	public CommandResult(IExecutable command, String text)
	{
		Command = command;
		Text = text;
	}
	
	public CommandResult(Direction direction, String text)
	{
		Command = MoveCommand.Instance;
		Direction = direction;
		Text = text;
	}
	
	public CommandResult(String text)
	{
		Text = text;
	}
	
	public CommandResult(String text, ItemType itemType)
	{
		Command = TakeCommand.Instance;
		Text = text;
		ItemsObtained.Add(itemType);
	}
	
	public CommandResult(String text, UseAction itemUse, List<ItemType> itemsObtained, List<ItemType> itemsLost)
	{
		Command = UseCommand.Instance;
		Text = text;
		ItemsObtained.AddRange(itemsObtained);
		ItemsLost.AddRange(itemsLost);
		UseAction = itemUse;
	}
	
	public CommandResult(String text, List<ItemType> itemsObtained, ItemType itemLost)
	{
		Command = InputCommand.Instance;
		Text = text;
		ItemsObtained.AddRange(itemsObtained);
		if (itemLost != null) ItemsLost.Add(itemLost);
	}
}
