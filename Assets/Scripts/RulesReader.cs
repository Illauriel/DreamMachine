using UnityEngine;
using System.Collections;

public class RulesReader {


	public static int[] GetStats(string[] handles, string sheet){
		int[] result = new int[handles.Length];
		string[] lines = sheet.Split('\n');
		for (int i = 0; i < lines.Length; i++) {
			string[] substrings = lines[i].Split('|');

			for (int j = 0; j < handles.Length; j++) {
				if (substrings[0] == handles[j]){
					result[j] = int.Parse(substrings[1]);
					Debug.Log("Found "+substrings[0]+" value" + substrings[1]);
				}

			}

		}
		Debug.Log(result[0]);
		return result;
	}
	public static string GetString(string handle, string sheet){
		string result = "";
		string[] lines = sheet.Split('\n');
		for (int i = 0; i < lines.Length; i++) {
			string[] substrings = lines[i].Split('|');
			if (substrings[0] == handle){
				result = substrings[1];
			}
		}
		return result;
	}
	public static Weapon GetWeapon(string id, string list){
		Weapon result = new Weapon();
		string[] lines = list.Split('\n');
		for (int i = 0; i < lines.Length; i++) {
			string[] substrings = lines[i].Split('|');


			if (substrings[0] == id){
				result.w_name = substrings[1];
				result.damage = substrings[2];
				result.damageType = Weapon.DamageType.Crushing;
				result.description = substrings[4];
			}

		}
		if (result.w_name == ""){
			return null;
		}
		return result;
	}
}
