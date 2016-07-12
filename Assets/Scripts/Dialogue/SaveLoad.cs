using UnityEngine;
using System.Collections;
using System.IO;


public class SaveLoad : MonoBehaviour {
	public  string savepath;
	// LevelResources data;
	// string filename = "savefile_1";
	LineInterpreter lin;
	//ScreenManager scr;
	//ADVHandler adv;
	AudioManager aud;
	//AnimationManager ani;

	LevelResources data;
	SpriteHolder spr_data;
	SoundHolder sound_data;

	public Sprite[] my_bcgs = new Sprite[0];
	public string[] my_dates = new string[0];
	public string[] my_places = new string[0];


	
	void Start(){
		lin = gameObject.GetComponent<LineInterpreter>();
		//scr = gameObject.GetComponent<ScreenManager>();
		//adv = gameObject.GetComponent<ADVHandler>();
		aud = gameObject.GetComponent<AudioManager>();
		//ani = gameObject.GetComponent<AnimationManager>();

		//show = true;
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android){
			savepath = Application.persistentDataPath+"/Saves";

		}
		else{
			savepath = Application.dataPath+"/Saves";

		}
		if ( !Directory.Exists(savepath)){
			Debug.LogWarning("Creating new save directory");
			Directory.CreateDirectory(savepath);
		}
		if (!File.Exists(savepath+"/"+"testsave"+".txt")){
			//File.Create(savepath+"/"+"testsave"+".txt");
			string nine = "1\n1\n1\n1\n1\n1\n1\n1\n1";
			StreamWriter sw = new StreamWriter(savepath+"/"+"testsave"+".txt");
			sw.WriteLine(nine);
			sw.Close();
		}

		Debug.Log("Save directory existance = " +Directory.Exists(savepath));
		//GetNames("testsave");

		data = SceneInitiator.GetData();
		spr_data = SceneInitiator.GetSpriteData();
		sound_data = SceneInitiator.GetSoundData();
	} 


	
	public void Save(string filename, int id){
		System.DateTime now = System.DateTime.Now;
		string savetext = "";
		/*if (adv.choice_on){
			for (int i = lin.line_id; i >= 0; i--) {
				string[] divided = lin.lines[i].Split(' ');
				Debug.Log("Rolling back\""+divided[0]);
				if (divided[0] == "menu:"){
					Debug.Log("Saving line number as "+i);
					savetext = savetext + i + "|";
					break;
				}
			}
		}
		else {
			savetext = savetext + lin.line_id + "|";
		}*/



		savetext = savetext + lin.cur_label + "|"; // Save label

		savetext = savetext + now.Day + "/" + now.Month + "/" + now.Year + "|"; // Save time
		/*if (scr.active_tags != null){	
			for (int i = 0; i < scr.active_tags.Length; i++) {
				if (scr.active_tags[i] != null){
					savetext = savetext + scr.active_tags[0] + "|" + scr.active_proprties[0] + "|";
					Debug.LogWarning("Saving tags and properties: "+ scr.active_tags[0] + "|" + scr.active_proprties[0]);
				}	
			}
		}
		else{
			savetext = savetext + "NULL" + "|" + "NULL" + "|";
			Debug.Log ("We couldn't do what you want");
		}*/
		//savetext = savetext + scr.active_tags[0] + "|" + scr.active_proprties[0] + "|";

		//Saving current sprites 
		/*for (int i = 0; i < scr.places.Length; i++) {
			int spr_id = -1;
			if (scr.places[i].sprite != spr_data.blank){ //only if not blank
				for (int j = 0; j < spr_data.sprites.Length; j++) {
					if (scr.places[i].sprite == spr_data.sprites[j]){
						spr_id = j;
					}
				}
			}
			if (spr_id == -1 && i == 0){
				savetext = savetext + "89" + "|";
			}
			else if (spr_id == -1){
				savetext = savetext + "-" + "|";
			}
		
			else{
				savetext = savetext + spr_id + "|";
			}
		}*/
		//Saving current playing audio
		for (int i = 0; i < aud.channels.Length; i++) {
			int clp_id = -1;
			if (aud.channels[i].source.clip != null && aud.channels[i].source.isPlaying){ //only if not blank and not playing
				Debug.Log ("SAVING AUDIO CLIP "+ aud.channels[i].source.clip.name);
				for (int j = 0; j < sound_data.sounds.Length; j++) {
					if (aud.channels[i].source.clip == sound_data.sounds[j]){
						clp_id = j;
					}
				}
			}
			if (clp_id == -1 && i != 0){
				savetext = savetext + "-" + "|";
			}

			else{
				savetext = savetext + clp_id + "|";
			}
		}
		for (int i = 0; i < aud.channels.Length; i++) {
		}
		//Saving variables
		//float variables
		for (int i = 0; i < data.floatvars.Length; i++) {
			savetext = savetext + data.floatvars[i] + "|";
		}
		//bool variables
		for (int i = 0; i < data.boolvars.Length; i++) {
			if (data.boolvars[i]){
				savetext = savetext + 1 + "|";
			}
			else{
				savetext = savetext+0+"|";
			}
		}
		//string variables
		for (int i = 0; i < data.stringvars.Length; i++) {
			savetext = savetext + data.stringvars[i] + "|";
		}


		string loadtext = "";

		loadtext = File.ReadAllText(savepath + "/" + filename + ".txt");	

		string[] substrings = loadtext.Split('\n');
		substrings[id] = savetext;
		loadtext = "";
		for (int i = 0; i < substrings.Length; i++) {
			if (i != substrings.Length-1){
				loadtext = loadtext+substrings[i]+'\n'; 
			}
			else{
				loadtext = loadtext+substrings[i];
			}
		}



		File.WriteAllText(savepath+"/"+filename+".txt", loadtext);

		
		//lin.testmessage = "Saved Loadout!";
	}
	
	public void Load(string filename, int id){

		string loadtext = File.ReadAllText(savepath+"/"+filename+".txt");	
	
		string[] substrings = loadtext.Split('\n');
		if (substrings[id] != ""){
			string[] parts = substrings[id].Split('|');

			/*int exp_length = 4 + scr.places.Length + aud.channels.Length 
				+ data.floatvars.Length + data.boolvars.Length + data.stringvars.Length;//expected save file length;
			if (parts.Length != exp_length){
				Debug.LogError("ACHTUNG! Save file from a different version. Can not load! " + parts.Length +" != "+ exp_length);
				return;
			}
			//Cease animation
			//lin.ani.FullStop();
			//lin.par.FullStop();
			//lin.aud.FullStop();
			adv.choice_on = false;
			aud.FullStop();
			gameObject.GetComponent<AnimationManager>().FullStop();
			//gameObject.GetComponent<ParticleManager>().FullStop();
			lin.par.FullStop();
			//gameObject.GetComponent<AudioManager>().FullStop();
			//WHY THE FUCK DID I WANT TO DESTROY POOR AUDIO SAUCES
			/*AudioSource[] sauces = gameObject.GetComponents<AudioSource>();
			for (int i = 0; i < sauces.Length; i++) {
				if (i>lin.aud.channels.Length-1){
					Destroy(sauces[i]);
				}
			}*/ 

			//lin.aud.temp_src = lin.aud.channels[0];
			//lin.aud.waiting = false;
			int var_no = 3;  // an iterator

			//manage screen
			/*scr.ClearScreen();
			for (int i = 0; i < scr.places.Length; i++) {
				if (parts[var_no] != "-" && parts[var_no] != "89"){
					int spr_id = int.Parse(parts[var_no]);
					scr.places[i].sprite = spr_data.sprites[spr_id];
				}
				if (parts[3] == "89"){
					scr.places[0].sprite = spr_data.blank;
					scr.under.color = Color.clear;

				}
				var_no++;
			}
			//Manage sound
			for (int i = 0; i < aud.channels.Length; i++) {
				if (parts[var_no] != "-"){

					int clp_id = int.Parse(parts[var_no]);
					Debug.Log("Assigning clip " + var_no + " " + parts[var_no] + " " + sound_data.sounds[clp_id].name);
					aud.channels[i].clip = sound_data.sounds[clp_id];
					aud.channels[i].Play();
				}
				var_no++;
			}
			//Restore variables

			for (int i = 0; i < data.floatvars.Length; i++) {
				data.floatvars[i] = float.Parse(parts[var_no]);
				var_no++;
			}
			//input bool variables
			for (int i = 0; i < data.boolvars.Length; i++) {
				if (parts[var_no] == "1"){
					data.boolvars[i] = true;
				}
				else{
					data.boolvars[i] = false;
				}
				var_no++;
			}
			//input string variables
			for (int i = 0; i < data.stringvars.Length; i++) {
				data.stringvars[i] = parts[var_no];
				var_no++;
			}

			//Place camera to origin
			gameObject.transform.parent.position = Vector3.zero;
			gameObject.transform.parent.eulerAngles = Vector3.zero;


			//scr.DrawImage(parts[3], parts[4]);
			lin.JumpToLabel(parts[1]);
			adv.back_log = "";
			int targ_line = int.Parse(parts[0]);
			lin.line_id = targ_line;
			lin.AnalyzeLine(lin.line_id);
			/*
			 * TEMPORARILY DISABLED
			 * int stepstogo = targ_line - lin.line_id;
			Debug.Log ("We shall list from line " + lin.line_id + " to " + parts[0]+ " for " + stepstogo + " steps.");

			for (int i = 0; i < stepstogo; i++) {
			
				if (lin.line_id < targ_line){
					string[] substrings = lin.lines[lin.line_id].Split(' ');
					if (substrings[0] != "play" && substrings[0] != "$")
					//interpreter may read a long chain of lines now 
					lin.AnalyzeLine(lin.line_id);
				}
				//Now collect if this iteratio we reached target line
				if (lin.line_id == targ_line){
					Debug.Log ("Load Success!");
					break;
				}
				else if (lin.line_id > targ_line){
					Debug.LogError("Something went wrong in loading");
					lin.line_id = targ_line;
					break;
				}
				if (lin.lines[lin.line_id].Contains("\"")){
					lin.line_id ++;
				}

			}*/


			//lin.testmessage = "Loaded Save!";
		}
	}
	
	public void GetNames(string filename){
		string loadtext = "";

		Sprite[] bcgs = new Sprite[0];
		string[] dates = new string[0];
		string[] places = new string[0];

		if (File.ReadAllText(savepath+"/"+"testsave"+".txt").Split('\n').Length<9){
			string nine = "1\n1\n1\n1\n1\n1\n1\n1\n1";
			Debug.Log("Incorrect line number in save file!");

			File.WriteAllText(savepath+"/"+"testsave"+".txt", nine);
		}
		loadtext = File.ReadAllText(savepath+"/"+filename+".txt");	

		
		string[] substrings = loadtext.Split('\n');
		bcgs = new Sprite[substrings.Length];
		dates = new string[substrings.Length];
		places = new string[substrings.Length];
		for (int i=0; i<substrings.Length; i++){
			string[] parts = substrings[i].Split('|');
			if (parts.Length>1){
				dates[i] = parts[2];
				//places[i] = parts[5];
			//	int geo_id = 5 + scr.places.Length + aud.channels.Length + data.floatvars.Length + data.boolvars.Length;
				//places[i] = parts[geo_id];

				//int spr_id = scr.FindSprite(parts[3], parts[4]);
				int spr_id = int.Parse(parts [3]);
				if (spr_id >= 0){
					//bcgs[i] = spr_data.sprites[spr_id]; 
				}
				else{
					//bcgs[i] = spr_data.sprites[89];//scr.data.blank;
				}
				Debug.Log("Saveload system found "+bcgs[i].name);
			}
			else{
				bcgs[i] = spr_data.blank;
				dates[i] = "Empty Entry";
				places[i] = "File no."+i;
			}
		}

		my_bcgs = bcgs;
		my_dates = dates;
		my_places = places;
		
	}

	public void Saver( int id){
		Save("testsave", id);
	}

	public void Loader (int id){
		Load("testsave", id);
	}

}
