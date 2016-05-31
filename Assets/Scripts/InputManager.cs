using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

	GameController game_controller;
	AgentController player;
	CameraController cam;
	Interact interact;

	//public bool inCombat;

	// Use this for initialization
	void Start () {
		GameObject player_obj = GameObject.FindGameObjectWithTag("Player");
		if (player_obj != null){
			player = player_obj.GetComponent<AgentController>();
		}
		else{
			Debug.LogError("Input Manager can't find player Controller");
		}
		cam = Camera.main.GetComponent<CameraController>();
		if (cam == null){
			Debug.LogError("A CameraController is not attached to the main camera!");
		}
		interact = Camera.main.GetComponent<Interact>();
		if (interact == null){
			Debug.LogError("An Interact script is not attached to the main camera!");
		}
		game_controller = gameObject.GetComponent<GameController>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)){
			//Vector3 destination = Vector3.zero;
			RayProbe();
		}
		if (Input.GetKeyDown(KeyCode.Space)){
			game_controller.TogglePause();
		}
	}


	void RayProbe(){
		RaycastHit hit = new RaycastHit();
		Ray ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
		int layerMask = 1 << 9;
		layerMask = ~layerMask;

		if (Physics.Raycast(ray, out hit, 100,layerMask)){
			//if the hit obj is an interactive object
			if (hit.collider.gameObject.layer == 8){
				//check distance
				Debug.Log(hit.collider.name);
				float interact_dist = Vector3.Distance(hit.transform.position, player.transform.position);
				Debug.Log(interact_dist);
				if (interact_dist <= 1.5f){
					interact.AnalyzeItem(hit);
				}
				else {
					player.MoveToPoint(hit.point);
				}
			}
			else {
				player.MoveToPoint(hit.point);
			}

		}
	}
}
