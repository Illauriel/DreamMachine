using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScriptReader : MonoBehaviour {

	//static ScreenManager scr;
	static AudioManager aud;
	static LevelResources global_data;
	static ResourceCheck res_check;

	public static string[] ProcessText(string in_text){
		Debug.Log ("Script Reader Processing text!");
		//scr = GameObject.Find("Resources").GetComponent<ScreenManager>();
		aud = GameObject.Find("DialogueController").GetComponent<AudioManager>();
		global_data = SceneInitiator.GetData();
		//CheckArrayLengths();

		string[] lines = in_text.Split('\n'); //split lines with linebreak
		for (int i=0;i<lines.Length; i++){
			lines[i] = FilterSpaces (lines[i]); //clear spaces
		}
		for (int i=0;i<lines.Length; i++){
			//Debug.Log("Filtering lineswitch in line " + i);
			lines[i] = FilterLineSwitch(lines, lines[i], i);
		}
		for (int i=0;i<lines.Length; i++){
			lines[i] = CleanLine(lines[i]);
		}
		for (int i=0;i<lines.Length; i++){

		}
		for (int i=0;i<lines.Length; i++){
			CheckLineValidity(i+1, lines[i]);
		}

		Debug.Log ("Game script successfuly processed");
		return lines;

	}




	static string FilterSpaces(string str){
		//Use to root out tabs and spaces before string
		char[] parsedline = str.ToCharArray();
		int skip = 0;
		for (int i=0;i<parsedline.Length; i++){
			if (parsedline[i] == ' ' ||parsedline[i] == '\t'){
				//parsedline[i] = '*';
				skip++;
			}
			else {break;}
		}
		
		return new string(parsedline, skip, parsedline.Length-skip);
	}

	static string FilterLineSwitch(string[] lines, string str, int line){
		//Use to root out Carriage return—new line inside string
		char[] parsedline = str.ToCharArray();
		//char opening = ' ';
		char closing = ' ';
		
		int skipline = 0; // number of lines to skip
		bool check = false; // do we need to start a search for a closing parentheses?
		bool matched = false; // a bool to skip a matched element
		for (int i=0;i<parsedline.Length; i++){
			if (parsedline[i] == '\"' && (i ==0 || parsedline[i-1] != '\\')){
				if(!matched){
					check = true;
					//opening = '\"';
					//parsedline[i] = ' ';
					closing = '\"';
					//Debug.Log(i);
				}
				matched = !matched;
				//Debug.Log (matched);
			}
			/*else if (parsedline[i] == '\'' && i != 0 && parsedline[i-1] != '\\'){
				if(!matched){
					check = true;
					opening = '\'';
					closing = '\'';
				}
				matched = !matched;
			}*/
			else if (parsedline[i] == '('){
				check = true;
				//opening = '(';
				closing = ')';
			}
			else if (parsedline[i] == '['){
				check = true;
				//opening = '[';
				closing = ']';
			}
			else if (parsedline[i] == '{'){
				check = true;
				//opening = '{';
				closing = '}';
			}
			//now find a closing parenth
			if (check){
				//Debug.Log(i);
				int find = FindClosingParent(parsedline, closing, i+1);
				if(find == -1){
					skipline++;

				//UNCOMMENT FOR TEST//	Debug.Log ("Iterating skipline " + skipline + " at line " + line + " for a symbol of " + closing + " numbred " + i);

					string temp = new string( parsedline )+ lines [line + skipline]; //+ FilterSpaces(lines[line+skipline]);
					lines[line+skipline] = "";
					parsedline = temp.ToCharArray();

				//UNCOMMENT FOR TEST//	Debug.Log(new string(parsedline));

					for (int j=line;j<lines.Length; j++){
						find = FindClosingParent(parsedline, closing, i+1);
						if(find == -1){
							skipline++;

						//UNCOMMENT FOR TEST//	Debug.Log ("Iterating skipline "+skipline+" at line "+line+ " for a symbol of "+closing+" numbred"+i);

							string huemp = new string(parsedline)+ lines [line + skipline]; //+ FilterSpaces(lines[line+skipline]);
							lines[line+skipline] = "";
							parsedline = huemp.ToCharArray();

						//UNCOMMENT FOR TEST//	Debug.Log(new string(parsedline));

						}
						else{
							check = false;
							
							//opening = ' ';
							closing = ' ';

						//UNCOMMENT FOR TEST//	Debug.Log ("found at symbol "+j);

							break;
						}
					}
				}
				else if (find>=0){
					check = false;
					//parsedline[find] = ' ';

				//UNCOMMENT FOR TEST//	Debug.Log ("found at symbol first and line "+line+" # "+i+" "+ find);

					//opening = ' ';
					closing = ' ';
					//Debug.Log ("found at symbol first and line "+line);
				}
				
			}
		} 
		
		
		
		return new string(parsedline);
	}
	static string CleanLine(string str){
		str = str.Replace("\\", "");
		return str;
	}
	static int FindClosingParent(char[] parsedline, char closing, int id){
		int char_id = -1;
		//int skipline = 0;
		for (int i=id;i<parsedline.Length; i++){
			if (parsedline[i] == closing && parsedline[i-1] != '\\'){
				char_id = i;
			}
		}
		
		return char_id;
		
	}

	static void CheckLineValidity(int line_no, string str){
		char[] chars = str.ToCharArray();
		string[] substrings = str.Split(' ');


		if (chars.Length > 0 && chars[0]!='\"'){
			if (str.Contains("\"") && !str.Contains("$")){ //excclude $ for variable skipping
				for (int i = 0; i < str.Length; i++) {
					if (chars[i] == '\"' && chars[i-1] == ' '){
						for (int j = 0; j < global_data.characters.Length; j++) {
							if (substrings[0] == global_data.characters[j].handle){
								return;
							}
						}
						Debug.LogError(line_no + ". Character handle "+substrings[0] + " not found");
						return;
					}
					else if (chars[i] == '\"' && chars[i-1] != ' '){
						Debug.LogError (line_no + ". Achtung! You have typed a character's name without a space!");
						return;
					}
				}
			}
			else if (chars[0] == '#' && chars[1] != ' '){
				Debug.LogError(line_no + ". Commentary without a space after # sign won't parse");
			}
			else if(substrings[0] == "play"){
				aud.FindChannel(line_no, substrings[1]);
				//aud.FindClip(line_no, substrings[2]);
				FindResource(line_no, res_check.sound_labels, substrings[2]); 
			}
			else if (substrings[0] == "stop"){
				aud.FindChannel(line_no, substrings[1]);
			}
			else if (substrings[0] == "show" || substrings[0] == "scene"){
				if (substrings.Length>2){

					for (int j = 1; j < substrings.Length-1; j++) {
						if (substrings [1+j] == "at"){
							break;
						}
						substrings[1] = substrings[1]+" "+substrings[1+j];
					}
					//scr.FindSprite(line_no, substrings[1], substrings[2]);
					FindResource(line_no, res_check.image_labels, substrings[1]);
				}
			}
			/*else if (substrings[0] == "hide"){
				for (int i = 0; i < global_data.spr_id_reg.Length; i++) {
					if (global_data.spr_id_reg[i] == substrings[1]){
						return;
					}
				}
				Debug.LogError(line_no + ". There is no image tag named " + substrings[1] + ".");
			}*/
		}
	}



	static public void InitiateResourceValidator(string in_text){
		GameObject res_obj = GameObject.Find("ResourceValidator");
		if (res_obj == null){
			res_obj = new GameObject("ResourceValidator");
		}
		
		res_check = res_obj.GetComponent<ResourceCheck>();
		if (res_check == null){
			res_check = res_obj.AddComponent<ResourceCheck>();
		}

		res_check.image_labels = new string[0];
		res_check.sound_labels = new string[0];

		string[] lines = in_text.Split('\n'); //split lines with linebreak
		for (int i=0;i<lines.Length; i++){
			lines[i] = FilterSpaces (lines[i]); //clear spaces
		}
		for (int i=0;i<lines.Length; i++){
			//Debug.Log("Filtering lineswitch in line " + i);
			lines[i] = FilterLineSwitch(lines, lines[i], i);
		}
		for (int i=0;i<lines.Length; i++){
			lines[i] = CleanLine(lines[i]);
		}
		for (int i=0;i<lines.Length; i++){
			string[] substrings = lines[i].Split(' ');
			if (substrings[0] == "image"){

				for (int j = 1; j < substrings.Length-1; j++) {
					substrings[1] = substrings[1]+" "+substrings[1+j];
				}
				List<string> imgs = new List<string>();
				imgs.AddRange(res_check.image_labels);
				imgs.Add(substrings[1]);
				res_check.image_labels = imgs.ToArray();

				Debug.Log(res_check.image_labels.Length + " "+ substrings[1]);
				//res_check.image_labels[res_check.image_labels.Length-1] = substrings[1];

			}
			if (substrings[0] == "sound"){

				List<string> snds = new List<string>();
				snds.AddRange(res_check.sound_labels);
				snds.Add(substrings[1]);
				res_check.sound_labels = snds.ToArray();

				//res_check.sound_labels = ArrayHandler.Add (res_check.sound_labels);
				Debug.Log(res_check.sound_labels.Length + " "+ substrings[1]);
				//res_check.sound_labels[res_check.sound_labels.Length-1] = substrings[1];
			}
		}

	}

	static void FindResource(int line_no, string[] list, string label){
		for (int i = 0; i < list.Length; i++) {
			if (list[i] == label){
				return;
			}
		}
		Debug.LogError(line_no + ". Resource validation for " + label + " failed");
	}

}
