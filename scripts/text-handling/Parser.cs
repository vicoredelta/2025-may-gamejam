using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class Parser
{
	String[] _lookAlias = ["look", "see", "examine", "inspect"];
	String[] _useAlias = ["use", "activate"];
	String[] _takeAlias = ["take", "grab", "pick", "get"];
	String[] _moveAlias = ["move", "walk"];
	String[] _helpAlias = ["help", "manual"];
	String[] _northAlias = ["north", "up"];
	String[] _southAlias = ["south", "down"];
	String[] _westAlias = ["west", "left"];
	String[] _eastAlias = ["east", "right"];
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
		else if (_helpAlias.Contains(words[0]))
		{
			command = Command.Help;
		}
		else
		{
			command = Command.InvalidCommand;
		}
		
		return new CommandInput(command, items, direction);
	}
}
