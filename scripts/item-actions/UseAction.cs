using Godot;
using System;
using System.Collections.Generic;

public class UseAction : ItemAction
{	
	public UseAction(String description, List<ItemType> requiredItems,
		List<ItemType> producedItems, List<ItemType> destroyedItems,
		ItemCreateLocation createLocation, bool requiresPower = false)
	{
		Description = description;
		ItemCreateLocation = createLocation;
		RequiredItems.AddRange(requiredItems);
		ProducedItems.AddRange(producedItems);
		DestroyedItems.AddRange(destroyedItems);
		RequiresPower = requiresPower;
	}
	
	public override CommandResult Execute(String inputText = "")
	{
		// Check that required items are available
		foreach (ItemType requiredItem in RequiredItems)
		{
			if (!(Player.Instance.HasItem(requiredItem) || Player.Instance.CurrentRoom.HasItem(requiredItem)))
			{
				return new CommandResult("There is no " + requiredItem.Name.ToLower() +
					" in vicinity.");
			}
		}
		
		// Produce new items
		foreach (ItemType producedItem in ProducedItems)
		{
			if (ItemCreateLocation == ItemCreateLocation.Player)
			{
				Player.Instance.Add(new Item(producedItem));
				ItemsGainedToIventory.Add(producedItem);
			}
			else
			{
				Player.Instance.CurrentRoom.Add(new Item(producedItem));
			}
		}
		
		// Destroy items
		foreach (ItemType destroyedItem in DestroyedItems)
		{
			if (Player.Instance.HasItem(destroyedItem))
			{
				Player.Instance.Take(destroyedItem);
				ItemsLostFromInventory.Add(destroyedItem);
			}
			else if (Player.Instance.CurrentRoom.HasItem(destroyedItem))
			{
				Player.Instance.CurrentRoom.Take(destroyedItem);
			}
		}
		
		return new CommandResult(Description, this, ItemsGainedToIventory, ItemsLostFromInventory);
	}
}
