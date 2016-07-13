using UnityEngine;
using System.Collections;

[RequireComponent (typeof (NavMeshAgent))]

public class AgentController : MonoBehaviour {
	//Controls AI and player characters outside combat with Navmesh and in combat
	//with A* tile-pathfindina

	NavMeshAgent agent;
	public float walkingSpeed;
	public float runningSpeed;



	public GameObject destMarker;
	GameObject marker;
	//Walk sounds
	public AudioClip[] stepSounds;
	float step_timer;
	AudioSource stepSrc;

	//Node movement
	AStar pathfinding;
	public Transform[] patrolNodes;
	int curNode;
	int movePoints;
	Vector3[] pathNodes;
	bool patroling;
	public bool destReached;

	//values for pausing
	bool sv_autobraking;
	Vector3 sv_velocity;

	// Use this for initialization
	void Start () {
		//gc = LevelInitiator.GetGameController();
		pathfinding = GameObject.FindObjectOfType<AStar>();
		agent = GetComponent<NavMeshAgent>();
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
		//Marker
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
	//Overload for slow walker
	public void MoveToPoint(Vector3 point, bool isWalking){
		MoveToPoint(point);
		if (isWalking){
			agent.speed = walkingSpeed;
		}
		else{
			agent.speed = runningSpeed;
		}
	}
	public void StrafeTo(Vector3 point){
		agent.destination = point;
		agent.autoBraking = true;
		agent.updateRotation = false;

	}

	public void MoveInPath(){
		
		if (agent.remainingDistance <= 0.1f){
			GoToNextNode();
		}

	}
	public void MoveInPath(float speed){
		MoveInPath();
		agent.speed = speed;
	}

	void GoToNextNode(){
		curNode++;
		movePoints--;
		if (curNode == pathNodes.Length-1){
			agent.autoBraking = true;
		}
		if (curNode >= pathNodes.Length){
			if (patroling){
				curNode = 0;
				agent.autoBraking = false;
			}
			else{
				curNode = pathNodes.Length-1;
				destReached = true;
			}
		}
		if (pathNodes.Length > 0){
			agent.destination = pathNodes[curNode];
			//agent.SetDestination(pathfinder.allNodes[Random.Range(0, pathfinder.allNodes.Length-1)].Pos);
		}
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
	public void StartPatrol(){
		pathNodes = new Vector3[patrolNodes.Length];
		for (int i = 0; i < patrolNodes.Length; i++) {
			pathNodes[i] = patrolNodes[i].position;
		}
		patroling = true;
	}	
	public void CleanPath(){
		pathNodes = new Vector3[0];
	}

	public void Attack(){
		
	}

	public void ApproachTarget(int trg){

		pathfinding.SetStart(transform.position);
		pathfinding.SetDestination(trg);
		Vector3[] appr_path = pathfinding.FindPath();
		if (appr_path.Length > movePoints){
			pathNodes = new Vector3[6];
			for (int i = 0; i < pathNodes.Length; i++) {
				pathNodes[i] = appr_path[i];
			}
		}
		else if (appr_path.Length > 0){
			pathNodes = new Vector3[appr_path.Length];
			for (int i = 0; i < pathNodes.Length; i++) {
				pathNodes[i] = appr_path[i];
			}
		}
		else {
			return;
		}
		patroling = false;
		destReached = false;
		agent.destination = pathNodes[0];
		MoveInPath(runningSpeed);
	}

	public int MovePoints{
		get {return movePoints;}
		set {movePoints = value;}
	}
}
