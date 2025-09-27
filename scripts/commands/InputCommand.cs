using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class InputCommand : Command
{
	// Make singleton
	private InputCommand() { }
	public static InputCommand Instance { get; private set; } = new InputCommand();
	
	public override CommandResult Execute(String[] words)
	{
		// Skip first word
		words = words.Skip(1).ToArray();
		
		List<ItemType> itemsFound = new List<ItemType>();
		String[] remainderText = Parser.AddNextItem(words, itemsFound, Player.Instance.GetItemsInVicinity());
		
		if (itemsFound.Count == 0)
		{
			if (remainderText.Length > 0)
			{
				return new CommandResult(this, "'" + remainderText[0] + "' is not an item in vicinity.");
			}
			else
			{
				return new CommandResult(this, "You must specify an [color=7b84ff]item[/color] in vicinity followed by the text input.");
			}
		}
		else
		{
			ItemType itemFound = itemsFound[0];
			
			if (remainderText.Length == 0)
			{
				return new CommandResult(this, "You need write some [color=de6ba5]input[/color] for " + itemFound.Name + ".");
			}
			else
			{
				InputAction action = Player.Instance.FindInputAction(itemFound);
				
				if (action == null)
				{
					return new CommandResult();
				}
				else
				{
					return action.Execute(remainderText[0]);
				}
			}
		}
	}
}
