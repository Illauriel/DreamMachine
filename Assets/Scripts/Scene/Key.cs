using UnityEngine;
using System.Collections;

public class Key : InteractiveItem {



	public string key_name;
	// Use this for initialization
	void Start () {
		//item_type = ItemType.Key;
		SetItemType(ItemType.Key);
	}
	
	public void Interact(){
		PlayInteractionSound();
		Destroy(gameObject);
	}
}
