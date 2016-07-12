using UnityEngine;
using System.Collections;

public static class SceneInitiator : object {

	//public static LevelResources data;

	/*// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}*/
	public static LevelResources GetData(){
		GameObject go = GameObject.Find("SceneResources");
		if (go != null){
			LevelResources result = go.GetComponent<LevelResourcesReference>().data;
			if (result != null){
				return result;
			}
			else{
				Debug.LogError("GameObject \"Scene Resources\" does not contain LevelResourcesReference component");
				return null;
			}
			 
		}
		else {
			Debug.LogError("GameObject \"Scene Resources\" was not found");
			return null;
		}
	}
	public static SpriteHolder GetSpriteData(){
		GameObject go = GameObject.Find("SceneResources");
		if (go != null){
			SpriteHolder result = go.GetComponent<SpriteHolder>();
			if (result != null){
				return result;
			}
			else{
				Debug.LogError("GameObject \"Scene Resources\" does not contain SpriteHolder component");
				return null;
			}
			
		}
		else {
			Debug.LogError("GameObject \"Scene Resources\" was not found");
			return null;
		}
	}

	public static SoundHolder GetSoundData(){
		GameObject go = GameObject.Find("SceneResources");
		if (go != null){
			SoundHolder result = go.GetComponent<SoundHolder>();
			if (result != null){
				return result;
			}
			else{
				Debug.LogError("GameObject \"Scene Resources\" does not contain SoundHolder component");
				return null;
			}
			
		}
		else {
			Debug.LogError("GameObject \"Scene Resources\" was not found");
			return null;
		}
	}

}
