using Godot;
using System;

public partial class Game : Node
{
	int redRodUseCount = 0;
	int blueRodUseCount = 0;
	int greenRodUseCount = 0;
	
	public void GeneratorPuzzle(CommandResult result)
	{
		if (result.UseAction == attachPowerCell)
		{
			World.Instance.CellIsPlaced = true;
			//console.Description = "Some kind of console. You hear the hum of its fan working beneath the casing.";
		}
		
		// Generator puzzle
		if (result.UseAction == useBlueRods || result.UseAction == useRedRods || result.UseAction == useGreenRods)
		{
			if (result.UseAction == useBlueRods)
			{
				if (blueRodUseCount++ < 2)
				{
					World.Instance.ShipPower += 20;
					OutputText("Ship power is increased by 20 units. Current level is " + World.Instance.ShipPower+ ".");
				}
				else
				{
					OutputText("This type of Rods currently won't provide more power. Try a different one!");
				}
			}
			else if (result.UseAction == useRedRods)
			{
				if (redRodUseCount++ < 2)
				{
					World.Instance.ShipPower += 30;
					OutputText("Ship power is increased by 30 units. Current level is " + World.Instance.ShipPower+ ".");
				}
				else
				{
					OutputText("This type of Rods currently won't provide more power. Try a different one!");
				}
			}
			else if (result.UseAction == useGreenRods)
			{
				if (greenRodUseCount++ < 2)
				{
					World.Instance.ShipPower += 15;
					OutputText("Ship power is increased by 15 units. Current level is " + World.Instance.ShipPower+ ".");
				}
				else
				{
					OutputText("This type of Rods currently won't provide more power. Try a different one!");
				}
			}
			
			if (World.Instance.ShipPower > 100)
			{
				World.Instance.ShipPower = 0;
				OutputText("As power increases to over 100 units the system short circuits and power level is reset to 0.");
				greenRodUseCount = 0;
				redRodUseCount = 0;
				blueRodUseCount = 0;
			}
			else if (World.Instance.ShipPower == 100)
			{
				World.Instance.IsPowerOn = true;
				AudioManager.Instance.PlaySFX("event_powercell");
				AudioManager.Instance.PlaybgAmbience("loop_0");
				OutputText("As power level reaches exactly 100 units power to the ship is completely restored.");
				Player.Instance.Take(redRods);
				Player.Instance.Take(blueRods);
				Player.Instance.Take(greenRods);
				Player.Instance.CurrentRoom.Take(redRods);
				Player.Instance.CurrentRoom.Take(blueRods);
				Player.Instance.CurrentRoom.Take(greenRods);
			}
			
			OutputText("\n");
		}
	}
}
