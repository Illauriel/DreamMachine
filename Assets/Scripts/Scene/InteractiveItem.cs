using UnityEngine;
using System.Collections;

public class InteractiveItem : MonoBehaviour {

	public enum ItemType {Key, Switch, Loot, Container, NPC};
	//[HideInInspector] public ItemType item_type;
	private ItemType item_type;

	public AudioClip[]interact_sounds;
	//public string interact_voice;
	//public string fail_voice;
	// Use this for initialization
	void Start () {
		gameObject.layer = 8;
	}
	


	public ItemType GetItemType(){
		return item_type;
	}

	public void SetItemType(ItemType newType){
		item_type = newType;
	}
	public void PlayInteractionSound(){
		if (interact_sounds.Length != 0){
			//set up src
			GameObject source_obj = new GameObject("TempAudio"+name);
			source_obj.transform.position = transform.position;
			AudioSource source = source_obj.gameObject.AddComponent<AudioSource>();
			source.spatialBlend = 1;

			//Chose sound randomly Slot 0 doesn't play so that sound doesn't repeat twice

			if (interact_sounds.Length > 1){
				int index = Random.Range(1, interact_sounds.Length);
				source.clip = interact_sounds[index];
				interact_sounds[index] = interact_sounds[0]; //replace just played sound with previously blocked one
				interact_sounds[0] = source.clip;
			}
			else {
				source.clip = interact_sounds[0];
			}
				
			source.Play();
			Destroy(source_obj, source.clip.length);
		}
	}
}
