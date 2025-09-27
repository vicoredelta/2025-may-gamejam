using Godot;
using System;
using System.Collections.Generic;

public class ItemType
{
	public ItemType(String name, String description, bool canBePickedUp, bool visible,
		String iconPath, String[] aliases)
	{
		Name = name;
		Description = description;
		CanBePickedUp = canBePickedUp;
		Visible = visible;
		IconPath = iconPath;
		for (int i = 0; i < aliases.Length; i++) aliases[i] = aliases[i].ToLower();
		Aliases.AddRange(aliases);
	}
	
	public String IconPath { get; }
	public String Name { get; }
	public List<String> Aliases { get; } = new List<String>();
	public String Description { get; set; }
	public bool CanBePickedUp { get; }
	public bool Visible { get; }
}

public class Item
{
	public Item(ItemType itemType)
	{
		Type = itemType;
	}
	
	public ItemType Type { get; private set; }
}
