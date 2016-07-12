using UnityEngine;
using System.Collections;
//using UnityStandardAssets.CrossPlatformInput;
//using UnityStandardAssets.Characters.FirstPerson;


public class Interact : MonoBehaviour {
	//public bool canPick;
	// Use this for initialization
	//bool isHiding;
	bool[] items;
	public string[] itemnames;
	//ExternalCameraManager excam;
	//FirstPersonController player;
	//Light hiding_light;
	PlayerVoice voice;
	//Flashlight flashlight;

	void Start(){
		items = new bool[itemnames.Length];
		//Debug.Log("Items "+items[0]);
		//excam = GetComponent<ExternalCameraManager>();
		//player = GetComponent<FirstPersonController>();
		GameObject player_obj = GameObject.FindGameObjectWithTag("Player");
		if (player_obj != null){
			voice = player_obj.GetComponent<PlayerVoice>();
			if (voice == null){
				Debug.LogError("PlayerVoice is missing on player!");
			}
		}

		//flashlight = GetComponentInChildren<Flashlight>();
	}
	//Update is called once per frame
	void Update () {


		//if (CrossPlatformInputManager.GetButtonDown("Take")){
			//if (!isHiding){
				//RayProbe(Camera.main.transform);
			//}
			/*else{
				excam.MoveBack(1);
				GetComponent<Collider>().enabled = true;
				isHiding = false;
				//hiding_light.enabled = false;
				player.controlEnabled = true;
				flashlight.maxRange = 10;
			}
		}*/

		//Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);//Camera.main.ScreenPointToRay(new Vector3(Screen.width/2, Screen.height/2, 0));
		//Debug.DrawRay (ray.origin, ray.direction, Color.green);
	}
	///	if (hit.collider.gameObject.layer.) {
	//		canPick = true;
	//	}
	//	else{
	//		canPick = false;
	//	}
	//}

	public void RayProbe(Transform cam){
		RaycastHit hit = new RaycastHit();
		float dist = 5.0f;
		//debug raycast
		//Vector3 forward = transform.TransformDirection(Vector3.forward)*2;
		Ray ray = Camera.main.ScreenPointToRay(new Vector3(Camera.main.pixelWidth/2, Camera.main.pixelHeight/2, 0));
		//Debug.DrawRay (transform.position, cam.forward, Color.green);
		//if(Physics.Raycast(cam.position, cam.forward, out hit, dist)){
		int layerMask = 1 << 11;
		layerMask = ~layerMask;
		if(Physics.Raycast(ray, out hit, dist, layerMask)){
			//dist = hit.distance;
			//Analyze shit
			Debug.Log(hit.collider.name);
			if(hit.collider.gameObject.layer == 8){
				Debug.Log("Layer passed");
				AnalyzeItem(hit);

				//Debug.Log (dist + " " + hit.collider.gameObject.name);
				//Destroy (hit.collider.gameObject);
				//return hit.collider.gameObject.name;
			}
		}
		//return "";
	
	}

	public void AnalyzeItem(RaycastHit hit){
		InteractiveItem item = hit.collider.GetComponent<InteractiveItem>();
		InteractiveItem.ItemType type = item.GetItemType();
		// if key item
		if (type == InteractiveItem.ItemType.Key){
			/*bool found = false;
			for (int i=0; i<itemnames.Length; i++){
				if (item.key_name == itemnames[i]){
					items[i] = true;
					found = true;
					PlayInteractionSound(item);
					Destroy(hit.collider.gameObject, item.interact_sound.length);
				}
			}
			if (!found){
				Debug.LogError ("The item named "+item.key_name+ " is not recognized!");
			}*/
			Key key_item =  hit.collider.GetComponent<Key>();;

			int key_id = FindQuestItem(key_item.key_name);
			if (key_id != -1){
				items[key_id] = true;
				/*if (item.interact_sound != null){
					//PlayInteractionSound(item);
					Destroy(hit.collider.gameObject, item.interact_sound.length);
				}
				else {
					Destroy(hit.collider.gameObject);
				}*/
				key_item.Interact();
				Debug.Log ("Picked up key "+key_id+" "+key_item.key_name);
			}

		}
		// if note
		else if (type == InteractiveItem.ItemType.Loot){
			//PlayInteractionSound(item);
		}
		// if switch obj
		else if (type == InteractiveItem.ItemType.Switch){
			//Cast type
			Switch sw_item =  hit.collider.GetComponent<Switch>();
			Debug.Log(sw_item);

			//Find keys
			int key_no = 0; // number of required key items
			if (sw_item.key_name != ""){
				string[] keynames = new string[1];

				//Check if mutiple keys
				if (sw_item.key_name.Contains(",")){
					keynames = sw_item.key_name.Split(',');
					key_no = keynames.Length;

				}
				else {
					keynames[0] = sw_item.key_name;
					key_no = 1;
				}
				Debug.Log("The switch has "+key_no+" required items");

				for (int i = 0; i < keynames.Length; i++) {
					int key_id = FindQuestItem(keynames[i]);
					if (key_id != -1){
						Debug.Log("Key " + itemnames[key_id] +" "+ items[key_id]);
						if (items[key_id]){
							Debug.Log("Key item found! " + itemnames[i] + " for keyname "+keynames[i]);
							key_no--;
						}
						else{
							Debug.LogWarning("You don't have key "+sw_item.key_name);
						}
					}
				}
				
			}

			//Sdelat' shto-nit
			if (key_no == 0){
				sw_item.Interact();
				if (sw_item.interactVoice != ""){
					voice.Play(sw_item.interactVoice);
				}
				//PlayInteractionSound(item);
				//Cheat for electricity

			}
			else{
				if (sw_item.failVoice != ""){
					Debug.Log(voice);
					voice.Play(sw_item.failVoice);
				}
			}

		}
		else if (type == InteractiveItem.ItemType.NPC){
			NPC npc = hit.collider.gameObject.GetComponent<NPC>();
			npc.Interact();
		}
		// if hiding object
		/*else if (item.type == InteractiveItem.ItemType.Hiding){
			if (!isHiding){
				//Transform in_pos = hit.transform.FindChild("pos1").transform;
				//Transform out_pos = hit.transform.FindChild("pos2").transform;
				excam.MoveTo(item.cam_nodes, 1);
				//hiding_light = item.GetComponentInChildren<Light>();
				//hiding_light.enabled = true;
				player.controlEnabled = false;
				GetComponent<Collider>().enabled = false;
				flashlight.FlashlightOff();
				flashlight.maxRange = 20;
				isHiding = true;
				PlayInteractionSound(item);
			}

		}*/
	}

	/*
	void PlayInteractionSound(InteractiveItem item){
		if (item.interact_sound != null){
			GameObject source_obj = new GameObject("TempAudio"+item.name);
			source_obj.transform.position = item.transform.position;
			AudioSource source = source_obj.gameObject.AddComponent<AudioSource>();
			source.spatialBlend = 1;

			source.clip = item.interact_sound;
			source.Play();
			Destroy(source_obj, source.clip.length);
		}
	}*/
	int FindQuestItem(string name_id){
		int found_id = -1;
		for (int i=0; i<itemnames.Length; i++){
			if (name_id == itemnames[i]){
				found_id = i;
			}
		}
		if (found_id == -1){
			Debug.LogError ("The item named "+name_id+ " is not recognized!");
		}
		return found_id;
	}
	public void SetItemActive(string name_id){
		int key_id = FindQuestItem(name_id);
		if (key_id != -1){
			if (items != null){
				items[key_id] = true;
				Debug.Log (name_id + " is on");
			}
			else {
				Debug.LogWarning("Items not yet initiated");
			}
		}
	}
	public void SetItemInactive(string name_id){
		int key_id = FindQuestItem(name_id);
		if (key_id != -1){
			if (items != null){
				items[key_id] = false;
				Debug.Log (name_id + " is off");
			}
			else {
				Debug.LogWarning("Items not yet initiated");
			}
		}
	}
}