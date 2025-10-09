using Godot;
using System;

public partial class TextOutput : RichTextLabel
{
	TextOutput()
	{
		// The strings below appear when the game boots up for the first time.
		AppendText("[color=efad42]Raid on the Sarcophagus Engine[/color]\n" +
			"a text adventure game by Chen Space Program\n\n" +
			
			"You are currently reading from the output window. You can type commands " +
			"into the input window at the bottom of the screen. " +
			"Typing commands is your only way to explore the game world. " +
			"If you need assistance, type '[color=de6ba5]help[/color]' or click on the ? icon.\n\n" +
			
			"==========================================================\n\n" +
			
			"Few know of the [color=efad42]antique spacecraft[/color] you stand before. It " +
			"crash-landed from its derelict orbit years ago, into an arid " +
			"basin, far from civilisation. Looters who tried their luck on " +
			"its spoils had returned empty-handed – or dead. They lacked the " +
			"means to open the craft's inner chambers: a [color=38a868]power cell[/color], to " +
			"revitalise the craft's inspirited machinery.\nYou have one such "+
			"cell in your possession.\nEntering the spacecraft was easy enough. " +
			"The crash had torn a convenient entrance in the hull, " + 
			"now only half concealed in tumbleweeds and sand.\n" +
			"Your eyes adjust quickly. You should [color=de6ba5]look[/color] around.\n\n");
	}
	
	public void TextOutputReceived(String textOutput)
	{
		AppendText(textOutput);
	}
}
