using Godot;
using System;

public partial class TextOutput : TextEdit
{
	public void TextOutputReceived(String textOutput)
	{
		InsertTextAtCaret(textOutput);
	}
}
