using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LineInterpreter : MonoBehaviour {

	public TextAsset resCheck;
	private TextAsset[] txt;
	public Localize[] locales;
	public int cur_locale = 1;

	public int line_id = 0;

	public string[] lines;

	public string cur_label;

	int[,] choices; // index 0 is start line index, index 1 is number of lines to go
	[HideInInspector] public bool menuOpen;

	//public string adv.back_log;

	//bool disabled;


	GUIManager gui;
	ADVHandler adv;
	ScreenManager scr;
	AudioManager aud;
	GameController gc;
	//public AnimationManager ani;
	//public ParticleManager par;
	//label search system
	int cur_doc;
	public string[,] labels;

	LevelResources data;

	void Awake () {
		data = SceneInitiator.GetData();
	}
	// Use this for initialization
	void Start () {
		adv = GameObject.FindObjectOfType<ADVHandler>();
		scr = GameObject.FindObjectOfType<ScreenManager>();
		aud = GameObject.FindObjectOfType<AudioManager>();
		gc  = GameObject.FindObjectOfType<GameController>();

		//ani = gameObject.GetComponent<AnimationManager>();
		gui = GameObject.FindObjectOfType<GUIManager>();

		ScriptReader.InitiateResourceValidator(resCheck.text);

		ActivateLocale (cur_locale);
		//AnalyzeLine(0);
		/*txt = locales[cur_locale].scenario; //assign text array
		labels = FindLabels(txt); // create a label map of all files
		lines = ScriptReader.ProcessText(txt[cur_doc].text); //get lines and cleanup current text file
		//WTF are we doing here?
		/*for (int i = 0; i < lines.Length; i++) {
			string[] substrings = lines[i].Split(' ');
			if (substrings[0] != "label"){
				line_id++;
			}
			else{
				break;
			}
		}*/
		//AnalyzeLine(0);


		Debug.Log("Line Interpreter initiated");
	}
	
	// Update is called once per frame
	void Update () {

		/*
		if (!stop){
			AnalyzeLine(line_id);
		}
		bool next = false;
		if (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.RightArrow)){
			next = true;
		}

		if (!adv.choicemenu[0].gameObject.activeSelf && adv.textbox.activeSelf && adv.arrow.activeSelf){
			if (adv.arrow.activeSelf){
				if(adv.input_delay<=0 && (fast || next || (Input.GetMouseButtonUp(0) && !new Rect(0,Screen.height*0.92f, Screen.width*0.34f, Screen.height*0.08f).Contains(Input.mousePosition) && !new Rect(Screen.width*0.8f, Screen.height*0.3f, Screen.width*0.2f, Screen.height*0.1f).Contains(Input.mousePosition)))){
					stop = false;
				}

			}

		}
		if (disabled && next){
			stop = false;

		}

		if (hide_all && hide_timer < 0 && next){
			hide_all = false;
			adv.enabled = true;
			menuon.SetActive(true);
			credits.SetActive(false);
		}
		if (hide_timer >0){
			hide_timer -= Time.deltaTime;
		}
		if ((Input.GetKey(KeyCode.LeftCommand) || Input.GetKey(KeyCode.LeftControl)) && Input.GetKeyDown(KeyCode.Backspace)){
			for (int i = line_id; i < lines.Length; i++) {
				string[] substrings = lines[i].Split(' ');
				if (substrings[0] == "label"){
					line_id = i;
					break;
				}
			}
		}

		adv.backlog.text = adv.back_log; */

	}


	public void AnalyzeLine(int id){
		string[] substrings = lines[id].Split(' ');
		if (substrings[0] == "menu:" ){
			//Debug.Log(id+ ". A wild MENU approaches!"); 
			//MenuManager(line_id);
			//NewMenuManager(id);
			ReadMenu(id);
			//AdvanceStep();
		} 
		else{
			AnalyzeLine(lines[id]);
		}
	}
	public void AnalyzeLine(string line){
		//checked for command line
		string[] substrings = line.Split(' ');
		//Debug.Log(substrings[0] + substrings[1]);
		//substrings[0] = "#";
		// all the damn cases of command use
		
		if (substrings[0] == "define"){
			//DefineChar(str); // ====== ====== ====== ====== ====== ====== DEFINING CHARS IN EDITOR ATM
		} 
		else if (substrings[0] == "#"){
			//Debug.Log(id + ". Comment string: "+lines[id]);
			AdvanceStep();
		}
		else if (substrings[0] == ""){
			//Debug.Log(id + ".     Empty line.");
			AdvanceStep();
		}
		else if (substrings[0] == "$"){
			//Debug.Log(id + ". Variable declaration! "+substrings[1]+ " "+substrings[3]);
			if (substrings.Length>=5){
				for (int i=0; i<substrings.Length-4; i++){
					substrings[3] = substrings[3]+" "+substrings[4+i];
				}
			}
			//SetVars(substrings[1], substrings[3]); 
			AdvanceStep();
		}
		else if (substrings[0] == "if" ){
			//Debug.Log(id + ". Variable query!");
			bool check = false;
			//check = GetVars(lines[id]);
			//if(!adv.GetVars(lines[id])){
			if(!check){
				line_id += 2;
				print("Skipping");
			}
			AdvanceStep();
		}
		else if (substrings[0] == "image"){
			//scr.LoadImage(str);// ====== ====== ====== ====== ====== ====== NOT LOADING ANYMORE
		} 
		else if (substrings[0] == "label"){
			cur_label = substrings[1];
			AdvanceStep();
		} 
		else if (substrings[0] == "jump"){
			JumpToLabel(substrings[1]);
			//AdvanceStep();
		}
		else if (substrings[0] == "play" ){
			if (substrings.Length < 4){
				//Debug.Log(id + ". Playing "+substrings[1]+" "+substrings[2]);
				aud.Play (substrings[1], substrings[2]);
			}
			else{

				if (substrings[3] == "delayed"){
					//Debug.Log(id + ". Audio "+substrings[1]+" "+substrings[2]+" queued to play. ");
					aud.PlayQueued(substrings[1], substrings[2]);
				}
				else if (substrings[3] == "crossfade"){
					//Debug.Log(id + ". Playing "+substrings[1]+" "+substrings[2]+" with crossfade "+substrings[4]);
					aud.PlayCrossfade(substrings[1], substrings[2], float.Parse(substrings[4]));

				}
				else{
					//Debug.Log(id + ". Playing "+substrings[1]+" "+substrings[2]+" with fadein "+substrings[4]);
					aud.Play (substrings[1], substrings[2], float.Parse(substrings[4]));
				}
			}
			AdvanceStep();
		} 
		else if (substrings[0] == "stop"){
			if (substrings.Length < 3){
				aud.Stop(substrings[1]);
			}
			else{
				aud.Stop(substrings[1], float.Parse(substrings[3]));
			}
			AdvanceStep();
		}
		else if (substrings[0] == "show" ){
			//Debug.Log(id+ ". Sprite " + substrings[1] + " changes to " + substrings[2]);
			if (!line.Contains(" at ")){
				for (int i = 1; i < substrings.Length-2; i++) {
					substrings[2] = substrings[2]+" "+substrings[2+i];
				}
				Debug.Log("Combined substring equals "+substrings[2]);
				scr.DrawImage(substrings[1], substrings[2]);
			}
			else{
				int at = 4;
				for (int i = 1; i < substrings.Length-3; i++) {
					if (substrings[2+i] != "at"){
						substrings[2] = substrings[2]+" "+substrings[2+i];
					}
					else{
						at = 3+i;
						break;
					}
				}
				Debug.Log("Combined substring equals "+substrings[2] );
				scr.DrawImage(substrings[1], substrings[2], substrings[at]);
			}
			AdvanceStep();

		} 
		else if (substrings[0] == "scene" /*&& !prepare*/){
			//scr.ClearScreen();
			if (substrings[1] == "bg"){
				//Debug.Log(id+ ". Scene" + substrings[1] + " changes to " + substrings[2]);
				//scr.DrawImage(substrings[1], substrings[2]);
			}
			else{
				if (substrings[1] == "black"){
					//scr.DrawColor("bg", Color.black);
				}
				else if (substrings[1] == "white"){
					//scr.DrawColor("bg", Color.white);
				}
				else if (substrings[1] == "clear"){
					//scr.DrawColor("bg", Color.clear);
				} 
			}
			AdvanceStep();
			//image.ReplaceTexture();
		} 
		else if (substrings[0] == "hide" ){
			//Debug.Log(id+ ". Hide " + substrings[1] );
			/*for (int i = 0; i < scr.places.Length; i++) {
				if (substrings[1] == scr.places[i].name){
					//scr.places[i].sprite = scr.data.blank;
					scr.active_tags[i] = "";
				}
			}
			scr.DrawImage(substrings[1]);*/
			AdvanceStep();
		} 
		else if (substrings[0] == "with" ){
			//Debug.Log(id+ ". Found a transition \"with "+ substrings[1]+"\"");

			//scr.Transition(substrings[1]);
			AdvanceStep();
		} 

		else if (substrings[0] == "return" ){
			JumpToLabel("start");
			Debug.LogWarning("Run finished in "+(Time.time/60)+ " minutes");
			AdvanceStep();
		} 
		else if (substrings[0] == "vibro" ){
			//Handheld.Vibrate();
			AdvanceStep();
		} 
		else if (substrings[0] == "animation" ){
			if (substrings[1]!= "stop"){
				//Debug.Log(id+ "Animation found");
				//ani.PlayAnimation(substrings[1], substrings[2]);
			}
			else{
				//ani.StopAnimation(substrings[2]);
			}
			AdvanceStep();
		} 
		else if (substrings[0] == "particle" ){
			if (substrings[1] != "stop"){
				//Debug.Log(id+ ". Particle found "+ substrings[1]);
				//par.ParticlePlay(substrings[1]);
			}
			else{
				//par.ParticleStop(substrings[2]);
			}
			AdvanceStep();
		}
		else if (substrings[0] == "disable" /*&& !prepare*/){
			//Debug.Log (id + ". Disabling ui");
		
			//gui.DisableUI();
			AdvanceStep();
		}
		else if (substrings[0] == "enable" /*&& !prepare*/){
			
		//	adv.input_delay = 0.5f;
			//adv.EnableUI();
			//gui.EnableUI();
			Debug.Log(line_id + ". Enabling main ui");
			//ani.PlayAnimation("adv", "show_adv");
			AdvanceStep();
		}
		else if (substrings[0] == "backlight" /*&& !prepare*/){
			//GameObject bl = adv.backlight.gameObject;
			//bl.SetActive(!bl.activeSelf);
			AdvanceStep();
		}
		else if (substrings[0] == "end"){
			gc.EndDialogue();
		}
		else { //Otherwise it's considered a text string

		/*	if (!scr.inTransit){
				scr.Transition("None");
			}*/
			//Debug.Log(line_id+ ". Dialogue line: " + lines[id]);
			if (line.Substring(0,1) != "\""){
				string[] separate = line.Split(' ');
				int chid = -1;
				for (int i = 0; i < data.characters.Length; i++) {
					if (data.characters[i].handle == separate[0]){
						chid = i;
					}
					//Debug.Log("Compare "+adv.data.handles[i] + " "+ separate[0]+ " " +(adv.data.handles[i] == separate[0]));
				}
				//Debug.Log(chid);
				adv.Say(chid, line.Substring(separate[0].Length+2, line.Length-separate[0].Length-3));
				adv.back_log += "\n"+data.characters[chid].charname + ": \""+ line.Substring(separate[0].Length+2, line.Length-separate[0].Length-3) + "\"\n";

				//adv.stop = true;
			}
			else{
				//adv.Say(lines[id]);
				//Debug.Log ("Saying "+lines[id].Substring(1, lines[id].Length-2));
				adv.Say(line.Substring(1, line.Length-2));
				adv.back_log += "\n" + line.Substring(1, line.Length-2) + "\n";
				//adv.stop = true;
			}


			//Debug.Log(adv.back_log.Length);

			/*Debug.Log (adv.back_log.Split('\n').Length);
			string[] changelog = adv.back_log.Split('\n');
			if (changelog.Length > 60){
				adv.back_log = "";
				int line_length_diff = changelog.Length - 60;
				for (int i=0; i<60 ; i++){
					adv.back_log += "\n"+changelog[i + line_length_diff];
				}
			}*/
			int loglimit = 5000;//3180;
			if (adv.back_log.Length > loglimit){
				int diff = adv.back_log.Length - loglimit;
				char[] chars = adv.back_log.ToCharArray();
				adv.back_log = new string(chars, diff, loglimit);

			}
		}
		
		

		//line_id++; //advance a line;

		
	}

	void AdvanceStep(){
		line_id++;
		AnalyzeLine(line_id);
	}
	/*public void JumpToLabel(string target){
		Debug.Log("Jumping to "+target);
		Debug.Log(lines.Length);
		for (int i = 0; i < lines.Length; i++) {
			string[] substrings = lines[i].Split(' ');

			//Debug.Log(i);
			//Debug.Log(substrings[0]);
			if (substrings[0] == "label"){
				Debug.Log("Found label " + substrings[1]);
			}
			if (substrings[0] == "label" && (substrings[1] == target+":" || substrings[1] == target)){
				line_id = i;
				Debug.Log("Label "+target+ " found at line "+ i);
				return;
			}
		}
		Debug.LogWarning("NOT FOUND!");
	}*/

	public void JumpToLabel(string target){
		Debug.Log("Jumping to "+target);
		if (!target.Contains(":")){
			target = target+":";
		}
		for (int i = 0; i < labels.GetLength(0); i++) {
			if (labels[i,0] == target){
				int doc_id = int.Parse(labels[i,1]);
				Debug.Log("Label "+target+ " found at line "+ labels[i,2] + " in doc "+doc_id);

				if (doc_id != cur_doc){
					cur_doc = doc_id;
					lines = ScriptReader.ProcessText(txt[cur_doc].text);
				}
				line_id = int.Parse(labels[i,2]);
				cur_label = labels[i,0];
				AnalyzeLine(line_id);
				return;

			}

		}
		Debug.LogError("Label named \""+target+"\" was not found!");
	}

	void ReadMenu(int id){
		menuOpen = true;
		List<string> options = new List<string>();
		List<int> option_ids = new List<int>();
		int step = id;
		while (step < lines.Length){
			if (lines[step].Contains("\"") && lines[step].Contains(":")){
				Debug.Log("Found item "+lines[step]);
				string cleanline = lines[step].Replace(":", "");
				options.Add(cleanline);
				option_ids.Add(step);
			}
			else if (lines[step].Contains("\"") && !lines[step].Contains(":") || lines[step].Contains("label")){
				Debug.Log("menu end at " + step);
				option_ids.Add(step);
				break;
			}
			step++;

		}
		choices = new int[option_ids.Count,2];
		for (int i = 0; i < option_ids.Count; i++) {
			Debug.Log(option_ids[i]);
			choices[i,0] = option_ids[i];
			if (i<option_ids.Count-1){
				choices[i,1] = option_ids[i+1]-option_ids[i]-2;
			}
			else{
				choices[i,1] = 0;
			}
		}
		gui.ChoiceMenu(options.ToArray());

	}

	void NewMenuManager(int id){
		//adv.stop = true;
		int[] menulines = new int[6];
		int itemno = 0;
		bool analyze = true;
		int step = id;
		string[] choices;
		int[] adress;
		int[] actions; //number of lines to read before skipping to end
		while(analyze){
			if (step<lines.Length){
				if (lines[step].Contains("\"") && lines[step].Contains(":")){
					Debug.Log("Found item "+lines[step]);
					menulines[itemno] = step;
					itemno++;
				}
				else if (lines[step].Contains("\"") && !lines[step].Contains(":") || lines[step].Contains("label")){
					Debug.Log("menu end at" + step);
					menulines[itemno] = step;
					analyze = false;
				}
			}
			else{
				Debug.Log("menu end at" + step);
				menulines[itemno] = step;
				analyze = false;
			}
			step++;
		}
		choices = new string[itemno];
		adress = new int[itemno];
		actions = new int[itemno];
		for (int i = 0; i < itemno; i++) {
			choices[i] = lines[menulines[i]];
			adress[i] = menulines[i];
			actions[i] = menulines[i+1]-menulines[i]-2;
		}

		//adv.ChoiceMenu(choices, adress, actions, menulines[itemno]);
	}
	public void ExecuteMenu(int choice_id){
		menuOpen = false;
		int adress = choices[choice_id,0];
		//line_id = choices[choice_id,0];
		for (int i = 0; i < choices[choice_id,1]; i++) {
			AnalyzeLine(adress+i+1);
			if (lines[adress+i+1].Contains("jump")){
				return;
			}
		}
		line_id = choices[choices.GetLength(0)-1, 0];
	}
	/*public void ExecuteMenu(int adress, int toread, int end){
		//int count = toread;
		bool jumped = false;
		line_id = adress;
		Debug.Log("Executing menu "+lines[adress]);
		for (int i = 0; i < toread; i++) {
			AnalyzeLine(adress+i+1);
			Debug.Log("Processing Line "+lines[adress+i+1]);
			if (lines[adress+i+1].Contains("jump")){
				Debug.Log("Abort sequence execution");
				jumped = true;
				break;
			}
		}
		if (!jumped){
			line_id = end;
		}
	}*/

	public void QuitGame(){
		Application.Quit();
	}

	string[,] FindLabels(TextAsset[] files){
		string[,] result;
		string[] clean_lines;
		List<string> labels_found = new List<string>();
		//int[] label_count;
		//int totalcount;
		//label_count = new int[files.Length];
		//First pass to get label amounts
		for (int i = 0; i < files.Length; i++) {
			clean_lines = ScriptReader.ProcessText(files[i].text);
			for (int j = 0; j < clean_lines.Length; j++) {
				string[] substrings = clean_lines[j].Split(' ');
				if (substrings[0] == "label"){
					labels_found.Add(substrings[1]+"|"+i+"|"+j);
				    //labels_found = ArrayHandler.Add(labels_found);
					//labels_found[labels_found.Length-1] = substrings[1]+"|"+i+"|"+j;
					//label_count[i]++;
					//totalcount++;
					//zDebug.Log((labels_found.Length-1)+" Label "+substrings[1]+" found");
				}
			}
			//Debug.Log("Scenario file #"+i+" has "+label_count[i]+ " labels");
		}
		Debug.Log("Scenario files hold "+labels_found.Count + " labels");
		result = new string[labels_found.Count,3];
		for (int i = 0; i < labels_found.Count; i++) {
			string[] substrings = labels_found[i].Split('|');
			result[i,0] = substrings[0];
			result[i,1] = substrings[1];
			result[i,2] = substrings[2];
		}
	
		return result;
	}


	void ActivateLocale(int loc_no){
		cur_locale = loc_no;
		txt = locales[cur_locale].scenario; //assign text array
		labels = FindLabels(txt); // create a label map of all files
		lines = ScriptReader.ProcessText(txt[cur_doc].text); //get lines and cleanup current text file

		if (line_id > 0){
			line_id --;
		}
		AnalyzeLine(line_id);
	}
}
