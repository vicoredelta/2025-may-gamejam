using Godot;
using System;

public partial class TextInput : TextEdit
{
	[Signal]
	public delegate void PlayerInputEventHandler(String textInput);
	
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("ui_accept"))
		{
			GD.Print("test");
			EmitSignal(SignalName.PlayerInput, "test");
		}
	}
}
