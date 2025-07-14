using Godot;
using System;
using System.Collections.Generic;

public class ItemUse
{
	String _description;
	List<ItemType> _requiredItems = new List<ItemType>();
	List<ItemType> _producedItems = new List<ItemType>();
	List<ItemType> _destroyedItems = new List<ItemType>();
	ItemCreateLocation _itemCreateLocation;
	
	public ItemUse(String description, List<ItemType> requiredItems,
		List<ItemType> producedItems, List<ItemType> destroyedItems,
		ItemCreateLocation createLocation)
	{
		_description = description;
		_itemCreateLocation = createLocation;
		_requiredItems.AddRange(requiredItems);
		_producedItems.AddRange(producedItems);
		_destroyedItems.AddRange(destroyedItems);
	}
	
	public List<ItemType> RequiredItems
	{
		get { return _requiredItems; }
	}
	
	public CommandOutput Use(Inventory playerInventory, Room currentRoom)
	{
		// Check that required items are available
		foreach (ItemType requiredItem in _requiredItems)
		{
			if (!(playerInventory.HasItem(requiredItem) ||
				currentRoom.HasItem(requiredItem)))
			{
				return new CommandOutput("There is no " + requiredItem.Name.ToLower() +
					" in inventory or vicinity.");
			}
		}
		
		// Produce new items
		foreach (ItemType producedItem in _producedItems)
		{
			if (_itemCreateLocation == ItemCreateLocation.Player)
			{
				playerInventory.Add(new Item(producedItem));
			}
			else
			{
				currentRoom.AddItem(new Item(producedItem));
			}
		}
		
		// Destroy items
		foreach (ItemType destroyedItem in _destroyedItems)
		{
			if (playerInventory.HasItem(destroyedItem))
			{
				playerInventory.Take(destroyedItem);
			}
			else if (currentRoom.HasItem(destroyedItem))
			{
				currentRoom.TakeItem(destroyedItem);
			}
		}
		
		return new CommandOutput(_description, _producedItems, _destroyedItems);
	}
}
