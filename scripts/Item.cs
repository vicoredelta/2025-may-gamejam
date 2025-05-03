using Godot;
using System;

public partial class Item : Node
{
	String name;
	String description;
	bool canBePickedUp;
	
	public Item(String name, String description, bool canBePickedUp)
	{
		this.name = name;
		this.description = description;
		this.canBePickedUp = canBePickedUp;
	}
	
	public string Name
	{
		get { return name; }
	}
	
	public string Description
	{
		get { return description; }
	}
	
	public bool CanBePickedUp
	{
		get { return canBePickedUp; }
	}
}
