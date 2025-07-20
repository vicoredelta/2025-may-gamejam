using Godot;
using System;
using System.Collections.Generic;

public class InputAction
{
	String _description;
	ItemType _requiredItem;
	List<ItemType> _producedItems = new List<ItemType>();
	ItemCreateLocation _itemCreateLocation;
	String _requiredText;
	String _wrongInputText;
	public bool RequiresPower{ get; }
	
	public InputAction(String description, String requiredText, String wrongInputText, ItemType requiredItem,
		List<ItemType> producedItems, ItemCreateLocation createLocation, bool requiresPower = false)
	{
		_description = description;
		_requiredText = requiredText;
		_itemCreateLocation = createLocation;
		_requiredItem = requiredItem;
		_wrongInputText = wrongInputText;
		RequiresPower = requiresPower;
	}
	
	public ItemType RequiredItem
	{
		get { return _requiredItem; }
	}
	
	public CommandOutput Activate(Player player, Room currentRoom, String inputText)
	{
		
		if (RequiresPower && !player._world.IsPowerOn)
		{
			return new CommandOutput("Nothing happens. It seems there's no power.");
		}
		ItemType itemLostFromInventory = null;
		List<ItemType> ItemsGainedToinventory = new List<ItemType>();
		
		// Check that required item is available
		if (!player.HasItem(_requiredItem) && !currentRoom.HasItem(_requiredItem))
		{
			return new CommandOutput("There is no " + _requiredItem.Name.ToLower() + " in vicinity.");
		}
		
		// Check if input text matches
		if (inputText.ToLower() != _requiredText.ToLower())
		{
			return new CommandOutput(_wrongInputText);
		}
		
		// Produce new items
		foreach (ItemType producedItem in _producedItems)
		{
			if (_itemCreateLocation == ItemCreateLocation.Player)
			{
				player.Add(new Item(producedItem));
				ItemsGainedToinventory.Add(producedItem);
			}
			else
			{
				currentRoom.AddItem(new Item(producedItem));
			}
		}
		
		// Destroy item
		if (player.HasItem(_requiredItem))
		{
			itemLostFromInventory = player.Take(_requiredItem).Type;
		}
		else if (currentRoom.HasItem(_requiredItem))
		{
			currentRoom.TakeItem(_requiredItem);
		}
		
		return new CommandOutput(_description, ItemsGainedToinventory, itemLostFromInventory);
	}
}
