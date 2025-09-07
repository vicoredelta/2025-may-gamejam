using Godot;
using System;
using System.Collections.Generic;

public abstract class ItemAction
{	
	protected String Description;
	protected List<ItemType> ProducedItems = new List<ItemType>();
	protected List<ItemType> DestroyedItems = new List<ItemType>();
	protected List<ItemType> ItemsLostFromInventory = new List<ItemType>();
	protected List<ItemType> ItemsGainedToIventory = new List<ItemType>();
	protected ItemCreateLocation ItemCreateLocation;
	
	public List<ItemType> RequiredItems { get; } = new List<ItemType>();
	public bool RequiresPower{ get; protected set; }
	
	public abstract CommandResult Execute(Player player, Room currentRoom, String inputText);
}
