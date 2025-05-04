using Godot;
using System;

public partial class TextOutput : RichTextLabel
{
	TextOutput()
	{
		// Set Intro text here
		AppendText("Few know of the antique spacecraft you stand before. It crash-landed from its derelict orbit years ago, into an arid basin, far from civilisation. Looters who tried their luck on its spoils had returned empty-handed – or dead. They lacked the means to open the craft’s inner chambers: a power cell, to revitalise the craft’s inspirited machinery.\nYou have one such cell in your possession.\nEntering the spacecraft was of little issue. The crash had torn a convenient entrance in the hull, now only half concealed in tumbleweeds and sand.\n");
	}
	
	public void TextOutputReceived(String textOutput)
	{
		AppendText(textOutput);
	}
}
