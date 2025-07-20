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
			
			foreach(String word in words.Skip(1))
			{
				if (_itemTypes.ContainsKey(word))
				{
					items.Add(_itemTypes[word]);
				}
			}
		}
		else if (_lookAlias.Contains(words[0]))
		{
			command = Command.Look;
			
			foreach(String word in words.Skip(1))
			{
				if (_itemTypes.ContainsKey(word))
				{
					items.Add(_itemTypes[word]);
					break;
				}
			}
		}
		else if (_takeAlias.Contains(words[0]))
		{
			command = Command.Take;
			
			foreach(String word in words.Skip(1))
			{
				if (_itemTypes.ContainsKey(word))
				{
					items.Add(_itemTypes[word]);
					break;
				}
			}
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
			bool itemFound = false;
			
			foreach(String word in words.Skip(1))
			{
				if (_itemTypes.ContainsKey(word) && !itemFound)
				{
					items.Add(_itemTypes[word]);
					itemFound = true;
				}
				else
				{
					entryText += word;
				}
			}
			
			
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
}
