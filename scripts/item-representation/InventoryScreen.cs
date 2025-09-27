using Godot;
using System;
using System.Collections.Generic;

public partial class InventoryScreen : ItemList
{
	public void UpdateInventory()
	{
		Clear();
		
		foreach (ItemType item in Player.Instance.GetItemTypes())
		{
			if (item.IconPath != "")
			{
				AddItem(item.Name, GD.Load<Texture2D>(item.IconPath), false);
			}
			else
			{
				AddItem(item.Name, null, false);
			}
		}
	}
}
