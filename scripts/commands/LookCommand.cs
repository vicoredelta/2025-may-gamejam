using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class LookCommand : CommandX
{
	private ItemType _itemToExamine;
	private bool _noRemainderText;
	
	// Make singleton
	private LookCommand() { }
	public static LookCommand Instance { get; private set; } = new LookCommand();
	
	public override CommandResult Execute(Player player, Room currentRoom)
	{
		// Continue from here
		if (_itemToExamine == null ||
			!(player.HasItem(_itemToExamine) || currentRoom.HasItem(_itemToExamine)))
		{
			String text = currentRoom.Description;
			
			if (currentRoom.ListItems() != "")
			{
				text = text + "\n" + currentRoom.ListItems();
			}
			
			return new CommandResult(Command.Look, text);
		}
		else
		{
			return new CommandResult(Command.Look, _itemToExamine.Description);
		}
	}
	
	public override void ParseInput(String[] words, Player player, Room currentRoom)
	{
		List <ItemType> list = new List<ItemType>();
		String[] remainderText = AddNextItem(words, list, GetItemsInVicinity(player, currentRoom));
		
		if (list.Count > 0)
		{
			_itemToExamine = list[0];
		}
		else
		{
			_itemToExamine = null;
		}
		
		_noRemainderText = (remainderText.Length == 0);
	}
}
