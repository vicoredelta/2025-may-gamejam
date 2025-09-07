using Godot;
using System;
using System.Collections.Generic;

public class UseCommand : Command
{
	// Make singleton
	private UseCommand() { }
	public static UseCommand Instance { get; private set; } = new UseCommand();
	
	public override CommandResult Execute(String[] words)
	{
		List <ItemType> itemsFound = new List<ItemType>();
		Parser.AddAllItems(words, itemsFound, Player.Instance.GetItemsInVicinity());
		
		if (itemsFound.Count == 0)
		{
			return new CommandResult("You must specify one, or several, [color=7b84ff]items[/color].");
		}
		else
		{
			UseAction foundUse = Player.Instance.FindUse(itemsFound);
	
			if (foundUse != null)
			{
				if (foundUse.RequiresPower && !World.Instance.IsPowerOn)
				{
					return new CommandResult("Nothing happens. Maybe it needs power?");
				}
				
				return foundUse.Execute();
			}
			
			return new CommandResult();
		}
	}
}
