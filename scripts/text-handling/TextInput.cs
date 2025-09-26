using Godot;
using System;

public partial class TextInput : TextEdit
{
	[Signal]
	public delegate void PlayerInputEventHandler();
	
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("input_accept"))
		{
			EmitSignal(SignalName.PlayerInput, GetLine(0));
			Clear();
		}
	}
}
