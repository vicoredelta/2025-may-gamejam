using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class Parser
{
	String[] _lookAlias = ["check", "examine", "inspect", "look", "see", "view"];
	String[] _useAlias = ["use", "activate"];
	String[] _takeAlias = ["get", "grab", "pick", "take"];
	String[] _moveAlias = ["move", "walk"];
	String[] _helpAlias = ["advice", "guide", "help", "hint", "manual", "tutorial"];
	String[] _northAlias = ["north", "n", "up", "u"];
	String[] _southAlias = ["south", "s", "down", "d"];
	String[] _westAlias = ["west", "w", "left", "l"];
	String[] _eastAlias = ["east", "e", "right", "r"];
	String[] _inputAlias = ["input", "entry", "write"];
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
			
			foreach(String word in words.Skip(1))
			{
				if (_northAlias.Contains(word))
				{
					direction = Direction.North;
					break;
				}
				if (_southAlias.Contains(word))
				{
					direction = Direction.South;
					break;
				}
				if (_westAlias.Contains(word))
				{
					direction = Direction.West;
					break;
				}
				if (_eastAlias.Contains(word))
				{
					direction = Direction.East;
					break;
				}
			}
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
		else
		{
			command = Command.InvalidCommand;
		}
		
		return new CommandInput(command, items, direction, entryText);
	}
	
	private void AddAllItems(String[] words, List<ItemType> items)
	{
		String[] remainingWords = words;
		while ((remainingWords = AddNextItem(remainingWords, items)).Length != 0);
	}
	
	private String[] AddNextItem(String[] words, List<ItemType> items)
	{
		int i = AddNextItemHelper(words, items);
		
		if (i == -1)
		{
			return [];
		}
		
		return words.Skip(i).ToArray();
	}
	
	private int AddNextItemHelper(String[] words, List<ItemType> items)
	{
		for (int i = 1; i <= words.Length; i++)
		{
			String[] segment = words.Take(i).ToArray();
			
			for (int j = (segment.Length - 1); j >= 0; j--)
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
