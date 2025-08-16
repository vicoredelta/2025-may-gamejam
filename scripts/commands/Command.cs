using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public interface IExecutable
{
	public CommandResult Execute(Player player, Room currentRoom);
	public void ParseInput(String[] words, Player player, Room currentRoom);
}

public abstract class CommandX : IExecutable
{
	public abstract CommandResult Execute(Player player, Room currentRoom);
	public abstract void ParseInput(String[] words, Player player, Room currentRoom);
	
	protected void AddAllItems(String[] words, List<ItemType> itemsFound, List<ItemType> itemsToSearch)
	{
		String[] remainingWords = words;
		while ((remainingWords = AddNextItem(remainingWords, itemsFound, itemsToSearch)).Length != 0);
	}
	
	// Returns remaining words after an item has been found
	protected String[] AddNextItem(String[] words, List<ItemType> itemsFound, List<ItemType> itemsToSearch)
	{
		int i = AddNextItemReturnIndex(words, itemsFound, itemsToSearch);
		
		if (i == -1)
		{
			return [];
		}
		
		return words.Skip(i).ToArray();
	}
	
	protected List<ItemType> GetItemsInVicinity(Player player, Room currentRoom)
	{
		List<ItemType> list = new List<ItemType>();
		
		list.AddRange(player.GetItemTypes());
		list.AddRange(currentRoom.GetItemTypes());
		return list;
	}
	
	// Returns index of next element in array after an item has been found
	private int AddNextItemReturnIndex(String[] words, List<ItemType> itemsFound, List<ItemType> itemsToSearch)
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
