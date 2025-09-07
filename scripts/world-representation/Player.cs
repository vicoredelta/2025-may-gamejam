using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Player : ItemHolder
{
	public World _world;
	List<UseAction> _uses = new List<UseAction>(); 
	List<InputAction> _inputActions = new List<InputAction>();
	
	public Room CurrentRoom { get; set; }
	
	public Player(Room startingRoom, World world = null)
	{
		CurrentRoom = startingRoom;
		_world = world;
	}
	
	public void AddUseAction(UseAction itemUse)
	{
		_uses.Add(itemUse);
	}
	
	public void AddInputAction(InputAction inputAction)
	{
		_inputActions.Add(inputAction);
	}
	
	public List<ItemType> GetItemsInVicinity()
	{
		List<ItemType> list = new List<ItemType>();
		
		list.AddRange(this.GetItemTypes());
		list.AddRange(CurrentRoom.GetItemTypes());
		return list;
	}
	
	public UseAction FindUse(List<ItemType> itemsProvided)
	{
		foreach (UseAction use in _uses)
		{
			bool itemsFound = true;
			
			foreach (ItemType reqItem in use.RequiredItems)
			{
				if (!itemsProvided.Contains(reqItem))
				{
					itemsFound = false;
					break;
				}
			}
			
			if (itemsFound)
			{
				return use;
			}
		}
		
		return null;
	}
	
	public InputAction FindInputAction(ItemType requiredItem)
	{
		return _inputActions.Find(x => x.RequiredItem == requiredItem);
	}
	
	public CommandResult ExecuteCommand(String inputText)
	{
		String[] words = inputText.ToLower().Split(' ');
		
		if (words.Length == 0)
		{
			return new CommandResult();
		}
		else
		{
			IExecutable command = Parser.GetCommand(words[0]);
			return command.Execute(words.Skip(1).ToArray(), this, CurrentRoom);
		}
	}
}
