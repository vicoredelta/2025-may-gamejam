using Godot;
using System;

public partial class ItemType
{
	String _name;
	String _description;
	bool _canBePickedUp;	
	
	public ItemType(String name, String description, bool canBePickedUp)
	{
		_name = name;
		_description = description;
		_canBePickedUp = canBePickedUp;
	}
	
	public String Name
	{
		get { return _name; }
	}
	
	public String Description
	{
		get { return _description; }
	}
	
	public bool CanBePickedUp
	{
		get { return _canBePickedUp; }
	}
}
