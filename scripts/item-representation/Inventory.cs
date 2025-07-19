using Godot;
using System;
using System.Collections.Generic;

public class Inventory
{
	List<Item> _items = new List<Item>();
	
	public int Count { get { return _items.Count; } }
	
	public bool HasItem(ItemType itemType)
	{
		return _items.Exists(x => x.Type == itemType);
	}
	
	public Item Take(ItemType itemType)
	{
		if (HasItem(itemType))
		{
			Item returnValue = _items.Find(x => x.Type == itemType);
			_items.Remove(returnValue);
			return returnValue;
		}
		else
		{
			return null;
		}
	}
	
	public void Add(Item item)
	{
		_items.Add(item);
	}
	
	public String ListItems()
	{
		String output = "";
		
		if (_items.Count != 0)
		{
			output += "There is ";
			
			if (_items.Count > 1)
			{
				for (int i = 0; i < _items.Count - 1; i++)
				{
					output += "a [" + _items[i].Type.Name.ToLower() + "], ";
				}
			}
			
			output += "a [" + _items[_items.Count - 1].Type.Name.ToLower() + "]";
		}
		
		return output;
	}
}
