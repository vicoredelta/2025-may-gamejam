using Godot;
using System;
using System.Collections.Generic;

public class Inventory
{
	protected List<Item> Items = new List<Item>();
	
	public int Count { get { return Items.Count; } }
	
	public bool HasItem(ItemType itemType)
	{
		return Items.Exists(x => x.Type == itemType);
	}
	
	public Item GetItem(ItemType itemType)
	{
		return Items.Find(x => x.Type == itemType);
	}
	
	public Item Take(ItemType itemType)
	{
		if (HasItem(itemType))
		{
			Item returnValue = Items.Find(x => x.Type == itemType);
			Items.Remove(returnValue);
			return returnValue;
		}
		else
		{
			return null;
		}
	}
	
	public void Add(Item item)
	{
		Items.Add(item);
	}
	
	public String ListItems()
	{
		String output = "";
		List<Item> filteredList = new List<Item>();
		filteredList = Items.FindAll(x => x.Type.Visible);
		
		if (filteredList.Count != 0)
		{
			output += "There is ";
			
			if (filteredList.Count > 1)
			{
				for (int i = 0; i < filteredList.Count - 1; i++)
				{
					output += "a [" + filteredList[i].Type.Name.ToLower() + "]";
					
					if (i != filteredList.Count - 2)
					{
						output += ", ";
					}
				}
				
				output += " and ";
			}
			
			output += "a [" + filteredList[filteredList.Count - 1].Type.Name.ToLower() + "]";
		}
		
		return output;
	}
}
