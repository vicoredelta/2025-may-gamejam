using Godot;
using System;

public partial class TextOutput : TextEdit
{
	TextOutput()
	{
		// Set Intro text here
		InsertTextAtCaret("You find yourself in an ancient ship. A convenient entrance created in the crash allowed you to enter with little issue.\n");
	}
	
	public void TextOutputReceived(String textOutput)
	{
		InsertTextAtCaret(textOutput);
	}
}
