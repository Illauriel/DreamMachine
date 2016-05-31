using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerVoice : MonoBehaviour {

	public AudioClip[] clips;
	public string[] clipnames;
	public string[] subtitles;
	public Text subs_text;
	float subs_time;

	void Start(){
		if (clips.Length != clipnames.Length || clips.Length != subtitles.Length || clipnames.Length != subtitles.Length){
			Debug.LogError("The player voice clip arrays and subtitles are not set up correctly!");
		}
	}

	void Update(){
		if (subs_time > 0){
			subs_time -= Time.deltaTime;
			//Debug.Log("SubsTime+" +subs_time);
		}	
		else if (subs_time <=0 && subs_time > -1){
			subs_time -= Time.deltaTime;
			subs_text.color = new Color(1, 1, 1, 1+subs_time);
		}
	}

	public void Play(string phrase_name){
		for (int i = 0; i < clipnames.Length; i++) {
			if (phrase_name == clipnames[i]){
				subs_time = 0;
				if (clips[i] != null) {
					//checking for multiple sources
					AudioSource source = null;
					AudioSource[] all_sources = gameObject.GetComponents<AudioSource>();
					for (int j = 0; j < all_sources.Length; j++) {
						if (all_sources[j].clip == clips[i]){
							source = all_sources[j];
						}
					}
					if (source == null){
						Debug.Log("Setting up voice");
						source = gameObject.AddComponent<AudioSource>();
						source.volume = 0.3f;
						source.spatialBlend = 1;
						source.clip = clips[i];
						source.Play();
						Destroy(source, clips[i].length);
						subs_time += clips[i].length;
					}
					else {
						Debug.Log("Ebanina desu");
					}
				}	
				ShowSubs(i);
			}
		}


	}

	void ShowSubs(int id){
		subs_text.text = subtitles[id];
		subs_text.color = Color.white;
		subs_time += 2;
	}

}
