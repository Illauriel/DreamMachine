using UnityEngine;
using System.Collections;

[System.Serializable]
public class AudioChannel {

	public AudioSource source;
	public string channelName; 
	public float channelVol;
	public bool autoLoop; //looped channels

}
