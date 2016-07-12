using UnityEngine;
using System.Collections;
using System;

public class LevelResources : ScriptableObject {
	public CharacterData[] characters;

	public string[] varnames;
	public float[] floatvars;
	public bool[] boolvars;
	public string[] stringvars;
	
	public Sprite blank;

	public string[] spr_id_reg; // Sprite Tag Register for saveload and script reading integrity checks
	public string[] snd_id_reg; // same for sound
}
