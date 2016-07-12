using UnityEngine;
using System.Collections;

public class Motor : ActivatableItem {

	public enum MotorState {Start, Running, Halt, Stopped, End};
	public MotorState cur_state;
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
	public bool stopIsHalt;

	//bool running;
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
		//moveTimer = transitionTime;
		if (playOnAwake){
			StartMotor();
		}

	}
	
	// Update is called once per frame
	void Update () {
		//if (running) {
		if (isRunning){
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

		if ((cur_state == MotorState.Start || cur_state == MotorState.End) && pingPong){
			waitTimer += Time.deltaTime;
			if (waitTimer > waitTime){
				StartMotor();
			}
		}
	}

	new public void Activate(){
		if (!isRunning){
			StartMotor();

		}
		else{
			if (stopIsHalt){
				HaltMotor();
			}
			else{
				
				StopMotor();
			}
		}
	}

	void StartMotor(){
		//Debug.Log("Moving "+obj.name);
		//running = true;
		if (cur_state != MotorState.Halt){
			reverse = !reverse;
		}
		waitTimer = 0;
		PlaySound();
		cur_state = MotorState.Running;
	}

	/*public void RestartMotor(){
		if ((reverse && moveTimer >= transitionTime)||moveTimer <= 0){
			//Nothing happens because moveTimer is over
		}
		else {
			cur_state = MotorState.Running;
			PlaySound();
		}

	}*/
	public void HaltMotor(){
		Debug.Log ("Halting "+obj.name);
		cur_state = MotorState.Halt;
		PauseSound();
	}

	void StopMotor(){
		Debug.Log ("Stopping "+obj.name);
		if (reverse && moveTimer > transitionTime){
			cur_state = MotorState.End;
		}
		else if (!reverse && moveTimer < 0){
			cur_state = MotorState.Start;
		}
		else{
			cur_state = MotorState.Stopped;
		
		}
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
		get{
			bool running = false;
			if (cur_state == MotorState.Running){
				running = true;
			}
			return running;
		}
	}
}
