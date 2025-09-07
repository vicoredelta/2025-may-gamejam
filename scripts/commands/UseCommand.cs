using Godot;
using System;
using System.Collections.Generic;

public class UseCommand : Command
{
	// Make singleton
	private UseCommand() { }
	public static UseCommand Instance { get; private set; } = new UseCommand();
	
	public override CommandResult Execute(String[] words, Player player, Room currentRoom)
	{
		List <ItemType> itemsFound = new List<ItemType>();
		Parser.AddAllItems(words, itemsFound, player.GetItemsInVicinity());
		
		if (itemsFound.Count == 0)
		{
			return new CommandResult("You must specify one, or several, [color=7b84ff]items[/color].");
		}
		else
		{
			UseAction foundUse = player.FindUse(itemsFound);
	
			if (foundUse != null)
			{
				if (foundUse.RequiresPower && !World.Instance.IsPowerOn)
				{
					return new CommandResult("Nothing happens. Maybe it needs power?");
				}
				
				return foundUse.Execute(player, currentRoom);
			}
			
			return new CommandResult();
		}
	}
}
