using Godot;
using System;

public partial class Game : Node
{
	[Signal]
	public delegate void TextOutputEventHandler();
	
	public void TextInputReceived(String textInput)
	{
		GD.Print("Game script received input");
		EmitSignal(SignalName.TextOutput, textInput + "\n");
	}
}
