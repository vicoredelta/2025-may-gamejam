using Godot;
using System;

public partial class TextOutput : TextEdit
{
	public void TextInputReceived(String textInput)
	{
		GD.Print("received!");
	}
}
