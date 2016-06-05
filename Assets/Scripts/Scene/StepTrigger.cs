using UnityEngine;
using System.Collections;

public class StepTrigger : MonoBehaviour {
	public string voice_id;
	PlayerVoice p_voice;
	public Switch triggerSwitch;
	public bool activateCombat;

	void Start(){
		GameObject player_obj = GameObject.FindGameObjectWithTag("Player");
		//PlayerVoice p_voice = null;
		if (player_obj != null){
			p_voice = player_obj.GetComponentInChildren<PlayerVoice>();
			//Debug.Log(p_voice);
			if (p_voice == null){
				Debug.LogError("Could not find player voice!!");
			}
		}
		else {
			Debug.LogError("Can't find player");
		}
	}

	void OnTriggerEnter(Collider col){
		if (col.tag == "Player"){
			//Debug.Log (p_voice);
			if (activateCombat){
				GameObject.Find("GameController").GetComponent<Pathfinding>().ActivateNodes(transform.position);
			}
			p_voice.Play(voice_id);

			if (triggerSwitch != null){
				triggerSwitch.Interact();
			
			}

			Destroy(gameObject);


		}
	}
}
