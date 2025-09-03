using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public static class ParserX
{
	// Command aliases
	static String[] _creditsAlias = ["author", "authors", "credits", "csp", "dev", "devs", "developer", "developers"];
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
	
	// Returns index of next element in array after an item has been found
	private static int AddNextItemReturnIndex(String[] words, List<ItemType> itemsFound, List<ItemType> itemsToSearch)
	{
		for (int i = 1; i <= words.Length; i++)
		{
			String[] segment = words.Take(i).ToArray();
			
			for (int j = 0; j < segment.Length; j++)
			{
				String potentialItemName = String.Join(" ", segment.Skip(j).ToArray());
				ItemType foundItem = itemsToSearch.Find(x => x.Name == potentialItemName);
				
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
