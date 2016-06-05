using UnityEngine;
using System.Collections;

public class Switch : InteractiveItem {



	public string key_name;
	public GameObject[] switchedObjects;
	public Motor[] activatedObjects;

	public string interactVoice;
	public string failVoice;
	public bool oneTime;
	bool done;
	// Use this for initialization
	void Start () {
		//item_type = ItemType.Switch;
		SetItemType(ItemType.Switch);
	}
	
	public void Interact(){
		//switch active status of switched objects
		if (!done){
			for (int i = 0; i < switchedObjects.Length; i++) {
				switchedObjects[i].SetActive ( ! switchedObjects[i].activeSelf );
			}
			for (int i = 0; i < activatedObjects.Length; i++) {
				activatedObjects[i].Activate();
			}
			//if the switch is single use, set it to inactive
			if (oneTime){
				done = true;
			}
			PlayInteractionSound();
		}
			
	}
}
