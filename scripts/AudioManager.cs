using Godot;
using System;
using System.Collections.Generic;

public partial class AudioManager : Node
{
	public static AudioManager Instance { get; private set; }

	public override void _EnterTree()
	{
		Instance = this;
	}
	// Exports audio dictionaries for use
	[Export] public AudioStreamPlayer bgAmbiencePlayer;
	[Export] public AudioStreamPlayer SFXPlayer;
	
	private Dictionary<string, AudioStream> sfxLibrary = new();
	private Dictionary<string, AudioStream> bgAmbienceLibrary = new();
	
	public override void _Ready()
	{
		// Dictionary with sound effect (SFX) used by the game
		sfxLibrary["walk"] = GD.Load<AudioStream>("res://assets/audio/sfx_walk_0.ogg");
		sfxLibrary["door_open"] = GD.Load<AudioStream>("res://assets/audio/sfx_door_open_0.ogg");
		sfxLibrary["event_powercell"] = GD.Load<AudioStream>("res://assets/audio/sfx_event_powercell_0.ogg");
		// Dictionary with background sound (bgs) used by the game
		bgAmbienceLibrary["loop_0"] = GD.Load<AudioStream>("res://assets/audio/bgs_ambience_loop_0.ogg");
		bgAmbienceLibrary["loop_1"] = GD.Load<AudioStream>("res://assets/audio/bgs_ambience_loop_1.ogg");
		bgAmbienceLibrary["loop_2"] = GD.Load<AudioStream>("res://assets/audio/bgs_ambience_loop_2.ogg");
		bgAmbienceLibrary["loop_3"] = GD.Load<AudioStream>("res://assets/audio/bgs_ambience_loop_3.ogg");
		bgAmbienceLibrary["loop_4"] = GD.Load<AudioStream>("res://assets/audio/bgs_ambience_loop_4.ogg");
		
		PlaybgAmbience("loop_4");
		
	}
	
	public void PlaybgAmbience (string name)
	{
		if (bgAmbienceLibrary.TryGetValue(name, out AudioStream bgAmb))
		{
			bgAmbiencePlayer.Stream = bgAmb;
			bgAmbiencePlayer.Play();
		}
		else
		{
			GD.PrintErr($" '{name}' not found!");
		}
	}
	
	public void PlaySFX (string name)
	{
		if (sfxLibrary.TryGetValue(name, out AudioStream sfx))
		{
			SFXPlayer.Stream = sfx;
			SFXPlayer.Play();
		}
		else
		{
			GD.PrintErr($" '{name}' not found!");
		}
	}
	
	
	
	
}
