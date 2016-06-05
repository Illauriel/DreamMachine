using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Elevator : MonoBehaviour {

	List<Transform> passengers;
	Motor myMotor;
	public NavMeshObstacle[] elevatorDoors;
	bool stopped;

	void Start(){
		passengers = new List<Transform>();
		myMotor = GetComponentInParent<Motor>();
		if (myMotor == null){
			Debug.Log("Warning, Elevator can't find Motor on "+transform.parent.name);
		}
	}

	void Update(){
		if (!stopped && !myMotor.isRunning){
			stopped = true;
			OpenDoor();

			for (int i = 0; i < passengers.Count; i++) {
				NavMeshAgent agent = passengers[i].GetComponent<NavMeshAgent>();
				if (agent != null){
					agent.destination = passengers[i].transform.position;
					agent.updatePosition = true;
				}
			}
		}
		else if (stopped && myMotor.isRunning){
			stopped = false;
			CloseDoors();
			for (int i = 0; i < passengers.Count; i++) {
				NavMeshAgent agent = passengers[i].GetComponent<NavMeshAgent>();
				if (agent != null){
					passengers[i].GetComponent<NavMeshAgent>().updatePosition = false;
				}
			}
		}
	}
	// Update is called once per frame
	void LateUpdate () {
		for (int i = 0; i < passengers.Count; i++) {
			if (myMotor.isRunning){
				
				Vector3 cur_pos = passengers[i].position;
				Vector3 new_pos = new Vector3(cur_pos.x, transform.position.y, cur_pos.z);
				passengers[i].position = new_pos;
			}
			else{
				
			}

		}
	}

	void OnTriggerEnter(Collider col){
		
		passengers.Add(col.transform);


	}
	void OnTriggerExit(Collider col){
		
		passengers.Remove(col.transform);

	}
	void CloseDoors(){
		for (int i = 0; i < elevatorDoors.Length; i++) {
			elevatorDoors[i].enabled = true;
		}
	}

	void OpenDoor(){
		int closest = -1;
		float closest_dist = Mathf.Infinity;
		for (int i = 0; i < elevatorDoors.Length; i++) {
			float dist = Vector3.Distance(transform.position, elevatorDoors[i].transform.position);
			if (dist < closest_dist){
				//Debug.Log(elevatorDoors[i].name + " is closer ");
				closest_dist = dist;
				closest = i;
			}
		}
		if (closest != -1){
			elevatorDoors[closest].enabled = false;
		}
	}

}
