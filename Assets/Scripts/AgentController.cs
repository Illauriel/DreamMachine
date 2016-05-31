using UnityEngine;
using System.Collections;

public class AgentController : MonoBehaviour {
	//Controls AI and player characters outside combat with Navmesh and in combat
	//with A* tile-pathfindina

	NavMeshAgent agent;
	public float walkingSpeed;
	public float runningSpeed;

	public AudioClip[] stepSounds;

	public GameObject destMarker;
	GameObject marker;
	float step_timer;
	AudioSource stepSrc;

	Pathfinding pathfinding;

	GameController gc;

	//values for pausing
	bool sv_autobraking;
	Vector3 sv_velocity;

	// Use this for initialization
	void Start () {
		gc = LevelInitiator.GetGameController();
		agent = LevelInitiator.GetAgent(gameObject);
		marker = (GameObject) Instantiate(destMarker, agent.transform.position, Quaternion.identity);
		stepSrc = gameObject.AddComponent<AudioSource>();
		stepSrc.spatialBlend = 1;
	}
	
	// Update is called once per frame
	void Update () {
		

		/* Vector2 input_axes = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

		if (input_axes != Vector2.zero){
			MoveInDirection (input_axes);
		}
		*/


		//stepsounds
		if (agent.velocity.magnitude > 0){

			step_timer -= agent.velocity.magnitude + Time.deltaTime;
			if (step_timer < 0){
				PlayStepSound();
				//Debug.Log(agent.velocity.magnitude);
				step_timer = 40f;
			}
		}

		if (marker.transform.localScale.y > 0){
			marker.transform.localScale -= Vector3.up * Time.deltaTime * 2;
			marker.transform.Translate(Vector3.down * Time.deltaTime);
		}
		else{
			//marker.transform.localScale = Vector3.zero;
			marker.SetActive(false);
		}

	}

	public void Pause(){
		//sv_autobraking = agent.autoBraking;
		sv_velocity = agent.velocity;
		agent.velocity = Vector3.zero;
		//agent.autoBraking = false;

		agent.Stop();	
	}
	public void Unpause(){
		agent.Resume();
		agent.velocity = sv_velocity;
	}

	public void MoveToPoint(Vector3 point){
		//Toggle between walking and running speed
		float point_dist = Vector3.Distance(agent.transform.position, point);

		if (point_dist < 4){
			agent.speed = walkingSpeed;
		}
		else {
			agent.speed = runningSpeed;
		}
		//Setting NavmeshAgent destination
		agent.destination = point;
		//agent.Resume();
		//Place destination marker at point
		marker.SetActive(true);
		marker.transform.position = agent.destination+Vector3.up; 
		marker.transform.localScale = Vector3.one* 0.2f + Vector3.up;
	}

	/*void MoveInDirection(Vector2 axes){
		//agent.transform.Translate(agent.transform.forward * Input.GetAxis("Vertical"));
		agent.Stop();
		Vector3 movement = new Vector3 (axes.x, 0, axes.y);
		Debug.Log(agent.transform.forward+ " " +Input.GetAxis("Vertical"));
		agent.transform.Translate(movement * walkingSpeed * Time.deltaTime);
		//agent.destination += agent.transform.forward * Input.GetAxis("Vertical");
		Debug.DrawRay(agent.transform.position, agent.transform.forward, Color.cyan);
	}*/
	void PlayStepSound(){
		if (stepSounds.Length > 0){
			int index = 0;
			if (stepSounds.Length > 1){
				index = Random.Range(1, stepSounds.Length);
				stepSrc.clip = stepSounds[index];
				stepSrc.PlayOneShot(stepSrc.clip);
				stepSounds[index] = stepSounds[0]; //replace just played sound with previously blocked one
				stepSounds[0] = stepSrc.clip;
			}
			else {
				//stepSrc.clip = stepSounds[0];
				stepSrc.PlayOneShot(stepSounds[0]);
			}


			//Destroy(src, src.clip.length);
		}
	}
		
}
