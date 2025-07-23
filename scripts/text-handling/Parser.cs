using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class Parser
{
	String[] _creditsAlias = ["author", "authors", "credits", "csp", "dev", "devs", "developer", "developers"];
	String[] _helpAlias = ["advice", "guide", "help", "hint", "manual", "tutorial"];
	String[] _inputAlias = ["input", "entry", "write"];
	String[] _lookAlias = ["check", "examine", "inspect", "look", "observe", "see", "view"];
	String[] _takeAlias = ["get", "grab", "pick", "take"];
	String[] _useAlias = ["use", "activate"];
	
	// Movement parsers
	String[] _moveAlias = ["go", "move", "sneak", "travel", "walk"];
	
	String[] _eastAlias = ["east", "e", "right", "r"];
	String[] _northAlias = ["north", "n", "up", "u"];
	String[] _southAlias = ["south", "s", "down", "d"];
	String[] _westAlias = ["west", "w", "left", "l"];
	
	Dictionary<String, ItemType> _itemTypes = new Dictionary<String, ItemType>();
	
	public Parser(List<ItemType> itemTypes)
	{
		foreach (ItemType itemType in itemTypes)
		{
			_itemTypes.Add(itemType.Name.ToLower(), itemType);
		}
	}
	
	public CommandInput GetCommand(String text)
	{
		String[] words = text.ToLower().Split(' ');
		List<ItemType> items = new List<ItemType>();
		Command command;
		Direction direction = Direction.InvalidDirection;
		String entryText = "";
		
		if (_useAlias.Contains(words[0]))
		{
			command = Command.Use;
			AddAllItems(words.Skip(1).ToArray(), items);
		}
		else if (_lookAlias.Contains(words[0]))
		{
			command = Command.Look;
			AddNextItem(words.Skip(1).ToArray(), items);
		}
		else if (_takeAlias.Contains(words[0]))
		{
			command = Command.Take;
			AddNextItem(words.Skip(1).ToArray(), items);
		}
		else if (_moveAlias.Contains(words[0]))
		{
			command = Command.Move;
			direction = GetDirection(words.Skip(1).ToArray());
		}
		else if (_inputAlias.Contains(words[0]))
		{
			command = Command.Input;
			String[] remainingText = AddNextItem(words.Skip(1).ToArray(), items);
			entryText += String.Join(" ", remainingText);
		}
		else if (_helpAlias.Contains(words[0]))
		{
			command = Command.Help;
		}
		else if (_creditsAlias.Contains(words[0]))
		{
			command = Command.Credits;
		}
		else
		{
			command = Command.InvalidCommand;
		}
		
		return new CommandInput(command, items, direction, entryText);
	}
	
	private Direction GetDirection(String[] words)
	{
		foreach(String word in words)
		{
			if (_northAlias.Contains(word)) return Direction.North;
			if (_southAlias.Contains(word)) return Direction.South;
			if (_westAlias.Contains(word)) return Direction.West;
			if (_eastAlias.Contains(word)) return Direction.East;
		}
		
		return Direction.InvalidDirection;
	}
	
	private void AddAllItems(String[] words, List<ItemType> items)
	{
		String[] remainingWords = words;
		while ((remainingWords = AddNextItem(remainingWords, items)).Length != 0);
	}
	
	// Returns remaining words after an item has been found
	private String[] AddNextItem(String[] words, List<ItemType> items)
	{
		int i = AddNextItemHelper(words, items);
		
		if (i == -1)
		{
			return [];
		}
		
		return words.Skip(i).ToArray();
	}
	
	// Returns index of next element in array after an item has been found
	private int AddNextItemHelper(String[] words, List<ItemType> items)
	{
		for (int i = 1; i <= words.Length; i++)
		{
			String[] segment = words.Take(i).ToArray();
			
			for (int j = 0; j < segment.Length; j++)
			{
				String potentialKey = String.Join(" ", segment.Skip(j).ToArray());
				
				if (_itemTypes.ContainsKey(potentialKey))
				{
					items.Add(_itemTypes[potentialKey]);
					return i;
				}
			}
		}
		
		return -1;	// return -1 when no match is found
	}
}
