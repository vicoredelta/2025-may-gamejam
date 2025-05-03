using Godot;
using System;

public partial class TextInput : TextEdit
{
	[Signal]
	public delegate void PlayerInputEventHandler();
	
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("ui_accept"))
		{
			EmitSignal(SignalName.PlayerInput, "test");
			Clear();
		}
	}
}
