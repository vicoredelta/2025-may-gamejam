using Godot;
using System;

public partial class Game : Node
{
	[Signal]
	public delegate void SetStasisImageVisibilityEventHandler();
	String stasisUnlockDescription = "The stasis pods unlock.";
	bool keycardFound = false;
	
	public void StasisPuzzle(CommandResult result)
	{
		if (Player.Instance.CurrentRoom.Name == "Stasis Chamber" &&
			result.Command == LookCommand.Instance && result.LookedAt == null)
		{
			EmitSignal(SignalName.SetStasisImageVisibility, true);
		}
		else
		{
			EmitSignal(SignalName.SetStasisImageVisibility, false);
		}
		
		if (result.UseAction == stasisUnlock && !keycardFound)
		{
			if (!World.Instance.StasisPodsUnlocked)
			{
				stasisUnlock.Description = "The pods are already unlocked.";
				World.Instance.StasisPodsUnlocked = true;
			}
		}
		
		if (result.UseAction != null && result.UseAction.RequiresStasisUnlock)
		{	
			if (result.UseAction == getKeycard && World.Instance.StasisPodsUnlocked)
			{
				stasisUnlock.Description = "There is no point in opening the stasis pods again, let the dead rest.";
				keycardFound = true;
			}
			else
			{
				stasisUnlock.Description = stasisUnlockDescription;
			}
			
			World.Instance.StasisPodsUnlocked = false;
		}
	}
}
