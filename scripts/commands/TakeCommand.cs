using Godot;
using System;
using System.Collections.Generic;

public class TakeCommand : Command
{
	// Make singleton
	private TakeCommand() { }
	public static TakeCommand Instance { get; private set; } = new TakeCommand();
	
	public override CommandResult Execute(String[] words)
	{
		List<ItemType> itemsFound = new List<ItemType>();
		Parser.AddNextItem(words, itemsFound, Player.Instance.GetItemsInVicinity());
		
		if (itemsFound.Count == 0)
		{
			return new CommandResult("You must specify an [color=7b84ff]item[/color] in the room.");
		}
		else
		{
			ItemType itemFound = itemsFound[0];
			
			if (!itemFound.CanBePickedUp)
			{
				return new CommandResult("You can not pick up the " + itemFound.Name.ToLower() + ".");
			}
			else
			{
				Player.Instance.CurrentRoom.Take(itemFound);
				return new CommandResult("You pick up the " + itemFound.Name.ToLower() + ".", itemFound);
			}
		}
	}
}
