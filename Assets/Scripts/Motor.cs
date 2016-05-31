using UnityEngine;
using System.Collections;

public class Motor : MonoBehaviour {
	public Transform obj;
	Vector3 startRotation;
	public AudioClip motionSound;
	public Vector3 targetRotation;

	Vector3 startPosition;
	public Vector3 targetPosition;

	public float transitionTime;
	public float waitTime;
	float moveTimer;
	float waitTimer;

	public bool pingPong;
	public bool playOnAwake;

	bool running;
	bool reverse;
	AudioSource src;


	void Start () {
		startPosition = transform.position;
		startRotation = transform.rotation.eulerAngles;
		//audio
		if (motionSound != null){
			src = gameObject.AddComponent<AudioSource>();
			src.spatialBlend = 1;
			src.loop = false;
			src.clip = motionSound;
		}

		if (playOnAwake){
			StartMotor();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (running) {
			if (reverse){
				moveTimer += Time.deltaTime;
				if (moveTimer > transitionTime){
					//running = false;
					//StopSound();
					StopMotor();
				}
			}
			else {
				moveTimer -= Time.deltaTime;
				if (moveTimer < 0){
					//running = false;
					//StopSound();
					StopMotor();
				}
			}



			//float cur_angle = max_angle * (moveTimer/open_time);
			//if (cw){
				//cur_angle = -cur_angle;
			//}
			float unit_time = moveTimer / transitionTime;

			obj.localRotation = Quaternion.Lerp(Quaternion.Euler(startRotation), Quaternion.Euler(targetRotation), unit_time);
			obj.localPosition = Vector3.Lerp(startPosition, targetPosition, unit_time);

		}

		if (!running && pingPong){
			waitTimer += Time.deltaTime;
			if (waitTimer > waitTime){
				StartMotor();
			}
		}
	}

	public void Activate(){
		if (!running){
			Debug.Log("Moving "+obj.name);
			StartMotor();

		}
		else{
			Debug.Log ("Stopping "+obj.name);
			StopMotor();
		}
	}

	public void StartMotor(){
		running = true;
		reverse = !reverse;
		waitTimer = 0;
		PlaySound();
	}

	public void RestartMotor(){
		if ((reverse && moveTimer >= transitionTime)||moveTimer <= 0){
			//Nothing happens because moveTimer is over
		}
		else {
			running = true;
			PlaySound();
		}

	}
	public void HaltMotor(){
		running = false;
		PauseSound();
	}

	public void StopMotor(){
		running = false;
		StopSound();

	}

	void PlaySound(){
		if (motionSound != null){
			src.Play();
		}
	}
	void StopSound(){
		if (motionSound != null){
			src.Stop();
		}
	}
	void PauseSound(){
		if (motionSound != null){
			src.Pause();
		}
	}

	public bool isRunning{
		get{return running;}
	}
}
