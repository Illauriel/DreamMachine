using UnityEngine;
using System.Collections;

public class NPC : InteractiveItem {



	public string key_name;
	//public GameObject[] switchedObjects;
	//public Motor[] activatedObjects;
	public string conv_label;
	public string interactVoice;
	//public string failVoice;
	//public bool oneTime;
	//bool done;
	// Use this for initialization
	LineInterpreter interp;
	GameController gc;

	void Start () {
		//item_type = ItemType.Switch;
		interp = FindObjectOfType<LineInterpreter>();
		gc = FindObjectOfType<GameController>();
		SetItemType(ItemType.NPC);
	}
	
	public void Interact(){
		gc.StartDialogue();
		interp.JumpToLabel(conv_label);
			
	}
}
