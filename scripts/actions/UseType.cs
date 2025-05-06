using Godot;
using System;
using System.Collections.Generic;

public partial class UseType
{
	ItemType item1;
	ItemType item2;
	List<ItemType> producedItems = new List<ItemType>();
	ItemsDestroyed itemsDestoyed;
	SpawnLocation createdAt;
	String description;
	
	public UseType(String description, ItemType item1, ItemType item2,
		ItemsDestroyed itemsDestoyed, ItemType[] producedItems = null,
		SpawnLocation createdAt = SpawnLocation.Automatic)
	{
		this.description = description;
		this.item1 = item1;
		this.item2 = item2;
		this.itemsDestoyed = itemsDestoyed;
		
		if (producedItems != null)
		{
			this.producedItems.AddRange(producedItems);
		}
		
		this.createdAt = createdAt;
	}
	
	public ItemType Item1
	{
		get { return item1; }
	}
	
	public ItemType Item2
	{
		get { return item2; }
	}
	
	public ItemType[] ProducedItems
	{
		get { return producedItems.ToArray(); }
	}
	
	public SpawnLocation CreatedAt
	{
		get { return createdAt; }
	}
	
	public String Description
	{
		get { return description; }
	}
}
