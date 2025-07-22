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
		List<Item> filteredList = new List<Item>();
		filteredList = _items.FindAll(x => x.Type.Visible);
		
		if (filteredList.Count != 0)
		{
			output += "\nThere is ";
			
			if (filteredList.Count > 1)
			{
				for (int i = 0; i < filteredList.Count - 1; i++)
				{
					output += "a [color=7b84ff]" + filteredList[i].Type.Name.ToLower() + "[/color]";
					
					if (i != filteredList.Count - 2)
					{
						output += ", ";
					}
				}
				
				output += " and ";
			}
			
			output += "a [color=7b84ff]" + filteredList[filteredList.Count - 1].Type.Name.ToLower() + "[/color]";
		}
		
		return output;
	}
}
