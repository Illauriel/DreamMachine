using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

	//public LevelResourcesa;
	public AudioChannel[] channels;

	public float volume; 
	public float[] fade; //
	public float[] seconds;
	//public int[] fresh; // indexes of freshly started clips/
	//queued Play
	public AudioSource temp_src;
	//public int mark_of_death;
	public float death_time;
	//public bool waiting;
	//crossfade
	public int cross_id = -1;
	//public float reftime;
	//public float crosstimer;
	public AudioSource crossed;
	SoundHolder sound_data;

	void Awake(){
		sound_data = SceneInitiator.GetSoundData();
	}

	void Start(){
		//channels = new AudioSource[channelNames.Length];
		seconds = new float[channels.Length];
		fade = new float[channels.Length];
		/*for (int i = 0; i < channelNames.Length; i++) {
			channels[i] = gameObject.AddComponent<AudioSource>();
			if (autoloop[i]){
				channels[i].loop = true;
			}

		}*/
	
		temp_src = gameObject.AddComponent<AudioSource>(); 
		crossed = gameObject.AddComponent<AudioSource>();

		//Debug.Log(sound_data);
		Debug.Log ("Audio Manager initiated");
	}


	void Update(){

		for (int i = 0; i < channels.Length; i++) {
			if (seconds[i] != 0){
				fade[i] -= Time.deltaTime;
				if (fade[i]<0){
					if (seconds[i]>0){
						channels[i].source.Stop();
					}
					fade[i] = 0;
					seconds[i] = 0;
				}
				if (seconds[i]>0){
					channels[i].source.volume = volume*(fade[i]/seconds[i]) * channels[i].channelVol;
				}
				else if (seconds[i]<0){
					channels[i].source.volume = volume*(1+fade[i]/seconds[i]) * channels[i].channelVol;
				}
			}

			else{
				channels[i].source.volume = volume * channels[i].channelVol; //volume x multiplier
			}
		}

		//Queued play
		if (death_time>0){
			death_time -= Time.deltaTime;
		}
		else{
			death_time = 0;
			temp_src.Stop();
		}

		//crossfade
		if (cross_id != -1 && fade[cross_id] > 0){
			crossed.volume = volume*(fade[cross_id]/-seconds[cross_id]) * channels[cross_id].channelVol;
			//Debug.Log(cross_id + " channel has volume of "+crossed.volume+ " and fades for" + fade[cross_id] +" = "+(fade[cross_id]/-seconds[cross_id]));
		}
		else if (cross_id != -1 && fade[cross_id] <= 0){
			fade[cross_id] = 0;
			crossed.volume = 0;
			crossed.Stop();
			cross_id = -1;
		}
		else {
			//crossed.volume = volume;
		}



	}

	public void PlayQueued (string channel, string str){
		AudioClip clip = FindClip(str);
		int chan_id = FindChannel(channel);
		
		if(chan_id != -1 && clip != null){
			temp_src.clip = channels[chan_id].source.clip;
			temp_src.time = channels[chan_id].source.time;
			temp_src.volume = volume * channels[chan_id].channelVol;
			temp_src.Play();

			death_time = channels[chan_id].source.clip.length-channels[chan_id].source.time;
			//Debug.LogWarning("Audio length is "+channels[chan_id].clip.length+" and delay is "+death_time);
			PlayDelayed(channels[chan_id].source, clip);
			//waiting = true;
			seconds[chan_id] = 0;
			fade[chan_id] = 0;
		}
		
	}
	public void PlayCrossfade(string channel, string str, float time){
		AudioClip clip = FindClip(str);
		int chan_id = FindChannel(channel);
		
		if(chan_id != -1 && clip != null){
			//switching older track to temp
			crossed.clip = channels[chan_id].source.clip;
			crossed.time = channels[chan_id].source.time;
			crossed.volume = channels[chan_id].source.volume;
			crossed.Play();
			//playing faded sound as expected
			PlaySound(channels[chan_id].source, clip);
			seconds[chan_id] = -time;
			fade[chan_id] = time;
			cross_id = chan_id;
		}
	}

	public void Play (string channel, string str){
		AudioClip clip = FindClip(str);
		int chan_id = FindChannel(channel);
		
		if(chan_id != -1 && clip != null){
			PlaySound(channels[chan_id].source, clip);
			seconds[chan_id] = 0;
			fade[chan_id] = 0;
		}
	}
	public void Play (string channel, string str, float fadein){
		AudioClip clip = FindClip(str);
		int chan_id = FindChannel(channel);

		if(chan_id != -1 && clip != null){
			PlaySound(channels[chan_id].source, clip);
			seconds[chan_id] = -fadein;
			fade[chan_id] = fadein;
		}
	}

	public void Stop (string channel){
		int chan_id = FindChannel(channel);
		if (chan_id >0){
			channels[chan_id].source.Stop();
		}
	}

	public void Stop (string channel, float fadeout){
		int chan_id = FindChannel(channel);
		if (chan_id >0){
			seconds[chan_id] = fadeout;
			fade[chan_id] = fadeout;
		}
	}

	public void PlaySound(AudioSource chan, AudioClip clip){
		chan.clip = clip;
		chan.Play();
		chan.volume = 0;
	}

	public void PlaySound(AudioSource chan, AudioClip clip, bool loop){
		chan.clip = clip;
		chan.Play();
		if (loop){
			chan.loop = true;
		}
		chan.volume = 0;
	}

	public void PlayDelayed(AudioSource chan, AudioClip clip){
		chan.clip = clip;

		chan.PlayScheduled(AudioSettings.dspTime + (double) death_time);
		chan.volume = 0;
	}

	public void FullStop(){
		Debug.LogError ("FOOOOLSTOOOP");
		//foreach (AudioSource x in channels){

		//x.Stop();
		for (int i = 0; i < channels.Length; i++) {
			if (channels[i].source.clip != null){
				Debug.Log (channels[i].source.clip.name);
				channels[i].source.Stop();
				channels[i].source.clip = null;
				Debug.Log (channels[i].source.isPlaying);
			}
		}
		crossed.clip = null;
		temp_src.clip = null;

	}

	public AudioClip FindClip(string str){
		Debug.Log("Looking for clip name "+str);
		Debug.Log("sound data = "+ sound_data.soundnames);
		for (int i = 0; i < sound_data.soundnames.Length; i++) {
			if (sound_data.soundnames[i] == str){
				//Debug.Log("Found clip "+str+"!");
				return sound_data.sounds[i];
			}
		}
		Debug.LogError("A clip with name "+str+" not found.");
		return null;
	}

	public int FindChannel(string str){
		for (int i = 0; i < channels.Length; i++) {
			if(channels[i].channelName == str){
				return i;
			}
		}
		Debug.LogError("A channel named "+str+" does not exist.");
		return -1;
	}
	//this one is for first run test
	public void FindClip(int id, string str){
		for (int i = 0; i < sound_data.soundnames.Length; i++) {
			if (sound_data.soundnames[i] == str){
				return;
			}
		}
		Debug.LogError(id + ". A clip with name "+str+" not found.");
	}

	public void FindChannel(int id, string str){
		for (int i = 0; i < channels.Length; i++) {
			if(channels[i].channelName == str){
				return;
			}
		}
		Debug.LogError(id + ". A channel named "+str+" does not exist.");
	}


}
