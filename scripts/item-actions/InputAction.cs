using Godot;
using System;
using System.Collections.Generic;

public class InputAction : ItemAction
{
	ItemType _requiredItem;
	String _requiredText;
	String _wrongInputText;
	
	public InputAction(String description, String requiredText, String wrongInputText, ItemType requiredItem,
		List<ItemType> producedItems, ItemCreateLocation createLocation, bool requiresPower = false)
	{
		Description = description;
		_requiredText = requiredText;
		ItemCreateLocation = createLocation;
		_requiredItem = requiredItem;
		_wrongInputText = wrongInputText;
		RequiresPower = requiresPower;
	}
	
	public ItemType RequiredItem
	{
		get { return _requiredItem; }
	}
	
	public override CommandResult Execute(Player player, Room currentRoom, String inputText)
	{
		
		if (RequiresPower && !player._world.IsPowerOn)
		{
			return new CommandResult("Nothing happens. It seems there's no power.");
		}
		ItemType itemLostFromInventory = null;
		List<ItemType> ItemsGainedToinventory = new List<ItemType>();
		
		// Check that required item is available
		if (!player.HasItem(_requiredItem) && !currentRoom.HasItem(_requiredItem))
		{
			return new CommandResult("There is no " + _requiredItem.Name.ToLower() + " in vicinity.");
		}
		
		// Check if input text matches
		if (inputText.ToLower() != _requiredText.ToLower())
		{
			return new CommandResult(_wrongInputText);
		}
		
		// Produce new items
		foreach (ItemType producedItem in ProducedItems)
		{
			if (ItemCreateLocation == ItemCreateLocation.Player)
			{
				player.Add(new Item(producedItem));
				ItemsGainedToinventory.Add(producedItem);
			}
			else
			{
				currentRoom.Add(new Item(producedItem));
			}
		}
		
		// Destroy item
		if (player.HasItem(_requiredItem))
		{
			itemLostFromInventory = player.Take(_requiredItem).Type;
		}
		else if (currentRoom.HasItem(_requiredItem))
		{
			currentRoom.Take(_requiredItem);
		}
		
		return new CommandResult(Description, ItemsGainedToinventory, itemLostFromInventory);
	}
}
