using UnityEngine;
using System.Collections;

public class ScreenManager : MonoBehaviour {

	public ScreenPlace[] places;

	SpriteHolder spr_data;

	// Use this for initialization
	void Start () {
		spr_data = FindObjectOfType<SpriteHolder>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}



	public void DrawImage(string tag, string id){
		int spr_id = FindSprite(tag, id);
		int plc_id = FindByTag(tag);
		if (spr_id < 0){
			return;
		}
		places[plc_id].active_tag = tag;
		places[plc_id].active_proprties = id;
		places[plc_id].image.sprite = spr_data.sprites[spr_id].sprite;
	}

	public void DrawImage(string tag, string id, string place){
		int spr_id = FindSprite(tag, id);
		int plc_id = FindPlace(place);
		if (spr_id < 0){
			return;
		}
		places[plc_id].active_tag = tag;
		places[plc_id].active_proprties = id;
		places[plc_id].image.sprite = spr_data.sprites[spr_id].sprite;
	}



	public int FindSprite (string tag, string id) {
		for (int i = 0; i < spr_data.sprites.Length; i++) {
			if (spr_data.sprites[i].spr_tags == tag && spr_data.sprites[i].spr_ids == id){
				return i;
			}
		}
		Debug.LogError("A combination of tag "+tag+" and id: "+id+" not found.");
		return -1;
	}

	int FindSprite (string tag) {
		for (int i = 0; i < spr_data.sprites.Length; i++) {
			if (spr_data.sprites[i].spr_tags == tag ){
				return i;
			}
		}
		Debug.LogError("Tag "+tag+"  not found."); 
		return -1;
	}
	//detection for bad tags and ID
	public void FindSprite (int line, string tag, string id) {
		for (int i = 0; i < spr_data.sprites.Length; i++) {
			if (spr_data.sprites[i].spr_ids == tag && spr_data.sprites[i].spr_ids == id){
				return;
			}
		}
		Debug.LogError(line + ". A combination of tag "+tag+" and id: "+id+" not found.");
	}

	public int FindPlace(string plc_name){
		for (int i = 0; i < places.Length; i++) {
			if(places[i].placeName == plc_name){
				return i;
			}
		}
		Debug.LogError ("Place named "+plc_name+" not found!");
		return -1;
	}
	public int FindByTag(string tag){
		int plc_id = -1;
		for (int i = 0; i < places.Length; i++) {
			if (places[i].active_tag == tag){
				plc_id = i;
			}	
		}
		if (plc_id == -1){
			if (tag == "bg"){
				plc_id = 0;
			}
			else {
				plc_id = 1;
				Debug.LogError ("Something went wrong while searching tag "+tag+"!");
			}
		}
		return plc_id;
	}
}
