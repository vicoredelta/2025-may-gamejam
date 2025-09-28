using Godot;
using System;

public partial class Game : Node
{
	int redTabletUseCount = 0;
	int blueTabletUseCount = 0;
	int greenTabletUseCount = 0;
	
	public void GeneratorPuzzle(CommandResult result)
	{
		if (result.UseAction == attachPowerCell)
		{
			World.Instance.CellIsPlaced = true;
			//console.Description = "Some kind of console. You hear the hum of its fan working beneath the casing.";
		}
		
		// Generator puzzle
		if (result.UseAction == useBlueTablets || result.UseAction == useRedTablets || result.UseAction == useGreenTablets)
		{
			if (result.UseAction == useBlueTablets)
			{
				if (blueTabletUseCount++ < 2)
				{
					World.Instance.ShipPower += 20;
					OutputText("Ship power is increased by 20 units. Current level is " + World.Instance.ShipPower+ ".");
				}
				else
				{
					OutputText("This type of tablets currently won't provide more power. Try a different one!");
				}
			}
			else if (result.UseAction == useRedTablets)
			{
				if (redTabletUseCount++ < 2)
				{
					World.Instance.ShipPower += 30;
					OutputText("Ship power is increased by 30 units. Current level is " + World.Instance.ShipPower+ ".");
				}
				else
				{
					OutputText("This type of tablets currently won't provide more power. Try a different one!");
				}
			}
			else if (result.UseAction == useGreenTablets)
			{
				if (greenTabletUseCount++ < 2)
				{
					World.Instance.ShipPower += 15;
					OutputText("Ship power is increased by 15 units. Current level is " + World.Instance.ShipPower+ ".");
				}
				else
				{
					OutputText("This type of tablets currently won't provide more power. Try a different one!");
				}
			}
			
			if (World.Instance.ShipPower > 100)
			{
				World.Instance.ShipPower = 0;
				OutputText("As power increases to over 100 units the system short circuits and power level is reset to 0.");
				greenTabletUseCount = 0;
				redTabletUseCount = 0;
				blueTabletUseCount = 0;
			}
			else if (World.Instance.ShipPower == 100)
			{
				World.Instance.IsPowerOn = true;
				AudioManager.Instance.PlaySFX("event_powercell");
				OutputText("As power level reaches exactly 100 units power to the ship is completely restored.");
				Player.Instance.Take(redTablets);
				Player.Instance.Take(blueTablets);
				Player.Instance.Take(greenTablets);
				Player.Instance.CurrentRoom.Take(redTablets);
				Player.Instance.CurrentRoom.Take(blueTablets);
				Player.Instance.CurrentRoom.Take(greenTablets);
			}
			
			OutputText("\n");
		}
	}
}
