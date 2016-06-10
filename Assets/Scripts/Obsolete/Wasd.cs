using UnityEngine;
using System.Collections;

public class Wasd : MonoBehaviour {
	public Transform cam;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.W)){
			//cam.Translate(Vector3.forward * Time.deltaTime);
			cam.transform.position += (Vector3.back+Vector3.right) * Time.deltaTime*3;
		}
		if (Input.GetKey(KeyCode.S)){
			//cam.Translate(Vector3.back * Time.deltaTime);
			cam.transform.position += (Vector3.forward + Vector3.left) * Time.deltaTime*3;
		}
		if (Input.GetKey(KeyCode.A)){
			//cam.Translate(Vector3.left * Time.deltaTime);
			cam.transform.position += (Vector3.forward + Vector3.right) * Time.deltaTime*3;
		}
		if (Input.GetKey(KeyCode.D)){
			//cam.Translate(Vector3.right * Time.deltaTime);
			cam.transform.position += (Vector3.back + Vector3.left) * Time.deltaTime*3;
		}
	}
}
