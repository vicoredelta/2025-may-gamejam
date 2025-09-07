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
	
	public override CommandResult Execute(Player player, Room currentRoom, String inputText = "")
	{
		// Check that required items are available
		foreach (ItemType requiredItem in RequiredItems)
		{
			if (!(player.HasItem(requiredItem) || currentRoom.HasItem(requiredItem)))
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
				player.Add(new Item(producedItem));
				ItemsGainedToIventory.Add(producedItem);
			}
			else
			{
				currentRoom.Add(new Item(producedItem));
			}
		}
		
		// Destroy items
		foreach (ItemType destroyedItem in DestroyedItems)
		{
			if (player.HasItem(destroyedItem))
			{
				player.Take(destroyedItem);
				ItemsLostFromInventory.Add(destroyedItem);
			}
			else if (currentRoom.HasItem(destroyedItem))
			{
				currentRoom.Take(destroyedItem);
			}
		}
		
		return new CommandResult(Description, this, ItemsGainedToIventory, ItemsLostFromInventory);
	}
}
