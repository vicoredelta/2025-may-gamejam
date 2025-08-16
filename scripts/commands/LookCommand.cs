using Godot;
using System;
using System.Collections.Generic;

public class LookCommand : CommandX
{	
	// Make singleton
	private LookCommand() { }
	public static LookCommand Instance { get; private set; } = new LookCommand();
	
	public override String[] Aliases { get; } =
		["check", "examine", "inspect", "look", "observe", "see", "view"];
	
	public override CommandResult Execute(String[] words, Player player, Room currentRoom)
	{
		List <ItemType> list = new List<ItemType>();
		String[] remainderText = AddNextItem(words, list, GetItemsInVicinity(player, currentRoom));
		
		if (list.Count > 0)
		{
			return new CommandResult(Command.Look, list[0].Description);
		}
		else if (remainderText.Length > 0)
		{
			return new CommandResult(Command.Look, remainderText[0] + " does not specify an " +
				"item in vicinity.");
		}
		else
		{
			return new CommandResult(Command.Look, currentRoom.GetRoomAndItemDescription());
		}
	}
}
