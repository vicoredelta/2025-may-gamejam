using Godot;
using System;
using System.Collections.Generic;

public class LookCommand : Command
{	
	// Make singleton
	private LookCommand() { }
	public static LookCommand Instance { get; private set; } = new LookCommand();
	
	public override CommandResult Execute(String[] words, Player player, Room currentRoom)
	{
		List <ItemType> list = new List<ItemType>();
		String[] remainderText = Parser.AddNextItem(words, list, player.GetItemsInVicinity());
		
		if (list.Count > 0)
		{
			// Output description of item if an item was specified
			return new CommandResult(this, list[0].Description);
		}
		else if (remainderText.Length > 0)
		{
			// Output error message if command was followed by text that does not include an item
			return new CommandResult(this, remainderText[0] + " does not specify an " +
				"item in vicinity.");
		}
		else
		{
			// Output description of room otherwise
			return new CommandResult(this, currentRoom.GetRoomAndItemDescription());
		}
	}
}
