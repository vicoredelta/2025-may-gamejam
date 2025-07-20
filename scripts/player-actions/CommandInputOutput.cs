using Godot;
using System;
using System.Collections.Generic;

public class CommandInput
{
	public Command Command { get; }
	public List<ItemType> Items { get; } = new List<ItemType>();
	public Direction Direction { get; } = Direction.InvalidDirection;
	public String EntryText { get; }
	
	public CommandInput(Command command, List<ItemType> items, Direction direction, String entryText)
	{
		Command = command;
		Direction = direction;
		Items.AddRange(items);
		EntryText = entryText;
	}
}

public class CommandOutput
{
	public String Text { get; } = "Nothing interesting happens.";
	public Command Command { get; } = Command.InvalidCommand;
	public List<ItemType> ItemsObtained { get; } = new List<ItemType>();
	public List<ItemType> ItemsLost { get; } = new List<ItemType>();
	public Direction Direction { get; } = Direction.InvalidDirection;
	public ItemUse ItemUse { get; } = null;
	
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
	
	public CommandOutput(String text, ItemUse itemUse, List<ItemType> itemsObtained, List<ItemType> itemsLost)
	{
		Command = Command.Use;
		Text = text;
		ItemsObtained.AddRange(itemsObtained);
		ItemsLost.AddRange(itemsLost);
		ItemUse = itemUse;
	}
	
	public CommandOutput(String text, List<ItemType> itemsObtained, ItemType itemLost)
	{
		Command = Command.Input;
		Text = text;
		ItemsObtained.AddRange(itemsObtained);
		if (itemLost != null) ItemsLost.Add(itemLost);
	}
}
