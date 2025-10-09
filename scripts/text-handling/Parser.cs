using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public static class Parser
{
	// Command aliases
	static String[] _creditsAlias = ["author", "authors", "credits", "csp", "dev", "devs", "developer", "developers", "whodunit"];
	static String[] _helpAlias = ["advice", "guide", "help", "hint", "manual", "tutorial"];
	static String[] _inputAlias = ["input", "entry", "write"];
	static String[] _lookAlias = ["check", "examine", "inspect", "look", "observe", "see", "view"];
	static String[] _takeAlias = ["get", "grab", "pick", "take"];
	static String[] _useAlias = ["use", "activate"];
	static String[] _moveAlias = ["go", "move", "sneak", "travel", "walk"];
	
	// Direction aliases
	static String[] _eastAlias = ["east", "e", "right", "r"];
	static String[] _northAlias = ["north", "n", "up", "u"];
	static String[] _southAlias = ["south", "s", "down", "d"];
	static String[] _westAlias = ["west", "w", "left", "l"];
	
	public static IExecutable GetCommand(String word)
	{
		if (_creditsAlias.Contains(word)) return CreditsCommand.Instance;
		if (_helpAlias.Contains(word)) return HelpCommand.Instance;
		if (_inputAlias.Contains(word)) return InputCommand.Instance;
		if (_lookAlias.Contains(word)) return LookCommand.Instance;
		if (_takeAlias.Contains(word)) return TakeCommand.Instance;
		if (_useAlias.Contains(word)) return UseCommand.Instance;
		if (_moveAlias.Contains(word)) return MoveCommand.Instance;
		return InvalidCommand.Instance;
	}
	
	// Add all items in 'itemsToSearch' to 'itemsFound' whose name match any element in 'words'
	public static void AddAllItems(String[] words, List<ItemType> itemsFound, List<ItemType> itemsToSearch)
	{
		String[] remainingWords = words;
		while ((remainingWords = AddNextItem(remainingWords, itemsFound, itemsToSearch)).Length != 0);
	}
	
	// Add next first item in  'itemsToSearch' to 'itemsFound' whose name match any element in 'words'
	// 'words' is array is iterated incrementally and this method returns remaining elements after an
	// item has been found
	public static String[] AddNextItem(String[] words, List<ItemType> itemsFound, List<ItemType> itemsToSearch)
	{
		int i = AddNextItemReturnIndex(words, itemsFound, itemsToSearch);
		
		if (i == -1)
		{
			return [];
		}
		
		return words.Skip(i).ToArray();
	}
	
	public static Direction GetDirection(String[] words)
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
	
	// Search 'itemsToSearch' for an item whose name is found in 'words' and add to 'itemsFound'.
	// Iterates through 'words' and returns index of next element in that array after an item has been found.
	private static int AddNextItemReturnIndex(String[] words, List<ItemType> itemsFound, List<ItemType> itemsToSearch)
	{
		for (int i = 1; i <= words.Length; i++)
		{
			String[] segment = words.Take(i).ToArray();
			
			for (int j = 0; j < segment.Length; j++)
			{
				String potentialItemName = String.Join(" ", segment.Skip(j).ToArray());
				ItemType foundItem;
				
				// Search for item whose name matches 'potentialItemName'
				foundItem = itemsToSearch.Find(x => x.Name.ToLower() == potentialItemName);
				
				if (foundItem == null)
				{
					// Search for item who has an alias that matches 'potentialItemName'
					foundItem = itemsToSearch.Find(x => x.Aliases.Contains(potentialItemName));
				}
				
				if (foundItem != null)
				{
					itemsFound.Add(foundItem);
					return i;
				}
			}
		}
		
		return -1;	// return -1 when no match is found
	}
}
