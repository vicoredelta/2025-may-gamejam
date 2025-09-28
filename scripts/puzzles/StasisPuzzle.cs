using Godot;
using System;

public partial class Game : Node
{
	[Signal]
	public delegate void SetStasisImageVisibilityEventHandler();
	
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
	}
}
