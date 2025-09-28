using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class UseCommand : Command
{
	// Make singleton
	private UseCommand() { }
	public static UseCommand Instance { get; private set; } = new UseCommand();
	
	public override CommandResult Execute(String[] words)
	{
		// Skip first word
		words = words.Skip(1).ToArray();
		
		List <ItemType> itemsFound = new List<ItemType>();
		Parser.AddAllItems(words, itemsFound, Player.Instance.GetItemsInVicinity());
		
		if (itemsFound.Count == 0)
		{
			return new CommandResult(this, "You must specify one or several [color=7b84ff]items[/color].");
		}
		else
		{
			UseAction foundUse = Player.Instance.FindUse(itemsFound);
	
			if (foundUse != null)
			{
				if (foundUse.RequiresPower && !World.Instance.IsPowerOn)
				{
					return new CommandResult(this, "Nothing happens. Maybe it needs power?");
				}
				
				if (foundUse.RequiresStasisUnlock && !World.Instance.StasisPodsUnlocked)
				{
					return new CommandResult(this, "The stasis pods are locked.");
				}
				
				if (foundUse.RequiresCell && !World.Instance.CellIsPlaced)
				{
					return new CommandResult(this, "Nothing happens. There is no power cell in place to interact with.");
				}
				
				return foundUse.Execute();
			}
			
			return new CommandResult();
		}
	}
}
