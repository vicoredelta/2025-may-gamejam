using Godot;
using System;

public partial class ItemType
{
	String name;
	String description;
	bool canBePickedUp;	
	
	public ItemType(String name, String description, bool canBePickedUp)
	{
		this.name = name;
		this.description = description;
		this.canBePickedUp = canBePickedUp;
	}
	
	public String Name
	{
		get { return name; }
	}
	
	public String Description
	{
		get { return description; }
	}
	
	public bool CanBePickedUp
	{
		get { return canBePickedUp; }
	}
}
