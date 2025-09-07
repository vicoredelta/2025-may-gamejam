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
	
	public override CommandResult Execute(String inputText)
	{
		
		if (RequiresPower && !World.Instance.IsPowerOn)
		{
			return new CommandResult("Nothing happens. It seems there's no power.");
		}
		ItemType itemLostFromInventory = null;
		List<ItemType> ItemsGainedToinventory = new List<ItemType>();
		
		// Check that required item is available
		if (!Player.Instance.HasItem(_requiredItem) && !Player.Instance.CurrentRoom.HasItem(_requiredItem))
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
				Player.Instance.Add(new Item(producedItem));
				ItemsGainedToinventory.Add(producedItem);
			}
			else
			{
				Player.Instance.CurrentRoom.Add(new Item(producedItem));
			}
		}
		
		// Destroy item
		if (Player.Instance.HasItem(_requiredItem))
		{
			itemLostFromInventory = Player.Instance.Take(_requiredItem).Type;
		}
		else if (Player.Instance.CurrentRoom.HasItem(_requiredItem))
		{
			Player.Instance.CurrentRoom.Take(_requiredItem);
		}
		
		return new CommandResult(Description, ItemsGainedToinventory, itemLostFromInventory);
	}
}
