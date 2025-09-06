using Godot;
using System;
using System.Collections.Generic;

public class InputCommand : CommandX
{
	// Make singleton
	private InputCommand() { }
	public static InputCommand Instance { get; private set; } = new InputCommand();
	
	public override CommandResult Execute(String[] words, Player player, Room currentRoom)
	{
		List<ItemType> itemsFound = new List<ItemType>();
		String[] remainderText = ParserX.AddNextItem(words, itemsFound, player.GetItemsInVicinity());
		
		if (itemsFound.Count == 0)
		{
			return new CommandResult("You must specify an [color=7b84ff]item[/color] in vicinity.");
		}
		else
		{
			ItemType itemFound = itemsFound[0];
			
			if (remainderText.Length == 0)
			{
				return new CommandResult("You need write some [color=de6ba5]input[/color] for " + itemFound.Name + ".");
			}
			else
			{
				InputAction action = player.FindInputAction(itemFound);
				
				if (action == null)
				{
					return new CommandResult();
				}
				else
				{
					return action.Activate(player, currentRoom, remainderText[0]);
				}
			}
		}
	}
}
