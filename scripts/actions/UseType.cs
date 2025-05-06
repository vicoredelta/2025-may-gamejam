using Godot;
using System;
using System.Collections.Generic;

public partial class UseType
{
	ItemType _item1;
	ItemType _item2;
	List<ItemType> _producedItems = new List<ItemType>();
	ItemsDestroyed _itemsDestoyed;
	SpawnLocation _spawnLocation;
	String _description;
	
	public UseType(String description, ItemType item1, ItemType item2,
		ItemsDestroyed itemsDestoyed, ItemType[] producedItems = null,
		SpawnLocation spawnLocation = SpawnLocation.Automatic)
	{
		_description = description;
		_item1 = item1;
		_item2 = item2;
		_itemsDestoyed = itemsDestoyed;
		
		if (producedItems != null)
		{
			_producedItems.AddRange(producedItems);
		}
		
		_spawnLocation = spawnLocation;
	}
	
	public ItemType Item1
	{
		get { return _item1; }
	}
	
	public ItemType Item2
	{
		get { return _item2; }
	}
	
	public ItemType[] ProducedItems
	{
		get { return _producedItems.ToArray(); }
	}
	
	public SpawnLocation spawnLocation
	{
		get { return _spawnLocation; }
	}
	
	public String Description
	{
		get { return _description; }
	}
}
