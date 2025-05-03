using Godot;
using System;

public partial class TextOutput : TextEdit
{
	public void TextInputReceived()
	{
		GD.Print("received!");
	}
}
