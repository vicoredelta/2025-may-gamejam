using Godot;
using System;
using System.Collections.Generic;

public class ItemUse
{
	String _description;
	List<ItemType> _requiredItems = new List<ItemType>();
	List<ItemType> _producedItems = new List<ItemType>();
	List<ItemType> _destroyedItems = new List<ItemType>();
	List<ItemType> _itemsLostFromInventory = new List<ItemType>();
	List<ItemType> _itemsGainedToIventory = new List<ItemType>();
	ItemCreateLocation _itemCreateLocation;
	public bool RequiresPower{ get; }
	
	public ItemUse(String description, List<ItemType> requiredItems,
		List<ItemType> producedItems, List<ItemType> destroyedItems,
		ItemCreateLocation createLocation, bool requiresPower = false)
	{
		_description = description;
		_itemCreateLocation = createLocation;
		_requiredItems.AddRange(requiredItems);
		_producedItems.AddRange(producedItems);
		_destroyedItems.AddRange(destroyedItems);
		RequiresPower = requiresPower;
	}
	
	public List<ItemType> RequiredItems
	{
		get { return _requiredItems; }
	}
	
	public CommandOutput Use(Player player, Room currentRoom)
	{
		// Check that required items are available
		foreach (ItemType requiredItem in _requiredItems)
		{
			if (!(player.HasItem(requiredItem) || currentRoom.HasItem(requiredItem)))
			{
				return new CommandOutput("There is no " + requiredItem.Name.ToLower() +
					" in vicinity.");
			}
		}
		
		// Produce new items
		foreach (ItemType producedItem in _producedItems)
		{
			if (_itemCreateLocation == ItemCreateLocation.Player)
			{
				player.Add(new Item(producedItem));
				_itemsGainedToIventory.Add(producedItem);
			}
			else
			{
				currentRoom.Add(new Item(producedItem));
			}
		}
		
		// Destroy items
		foreach (ItemType destroyedItem in _destroyedItems)
		{
			if (player.HasItem(destroyedItem))
			{
				player.Take(destroyedItem);
				_itemsLostFromInventory.Add(destroyedItem);
			}
			else if (currentRoom.HasItem(destroyedItem))
			{
				currentRoom.Take(destroyedItem);
			}
		}
		
		return new CommandOutput(_description, this, _itemsGainedToIventory, _itemsLostFromInventory);
	}
}
