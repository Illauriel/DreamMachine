using UnityEngine;
using System.Collections;

public class RotationActivator : MonoBehaviour {

	Motor hinge;
	// Use this for initialization
	void Start () {
		Transform root =  transform.root;
		if (root != null){
			hinge = root.GetComponent<Motor>();
			Debug.Log (hinge);
			Debug.Log (root.name);
		}

	}

	void OnEnable(){
		ActivateDoor();
	}

	void OnDisable(){
		ActivateDoor();

	}

	void ActivateDoor(){
		if (hinge != null){
			//if (!hinge.running){
				Debug.Log("Moving "+hinge.name);
				//hinge.OpenDoor();
				
			//}
			//else{
				Debug.Log ("Stopping "+hinge.name);
				//hinge.StopDoor();
			//}
		}
	}

}
