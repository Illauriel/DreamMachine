using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIPerception : MonoBehaviour {

	private Plane x_plane;
	private Plane z_plane;

	public float spotDist;
	public float fov;
	Transform[] enemies;
	//Spotted target
	Transform[] targets;

	// Use this for initialization
	void Start () {
		//Find all enemy targets
		Transform[] temp_targs = new Transform[0];
		if (tag == "Enemy"){
			temp_targs = LevelInitiator.FindObjectsOfTypeWithTag<Transform>("Ally");
			Transform player = GameObject.FindWithTag("Player").transform;
			enemies = new Transform[temp_targs.Length+1];
			enemies[enemies.Length-1] = player;
		}
		else if (tag == "Ally"){
			temp_targs = LevelInitiator.FindObjectsOfTypeWithTag<Transform>("Enemy");
			enemies = new Transform[temp_targs.Length];
		}

		for (int i = 0; i < temp_targs.Length; i++) {
			enemies[i] = temp_targs[i];
		}
		//set fov
		SetFOV(fov);
		targets = new Transform[0];
	}

	// Update is called once per frame
	void Update () {
		
		Sight();


	}
	void Sight(){
		bool showing_cone = false;
		for (int i = 0; i < enemies.Length; i++) {
			float distance = Vector3.Distance (transform.position, enemies[i].position);
			if (distance <= spotDist*1.2f){
				if (!showing_cone){
					DrawVisionCone(4);	
				}
			}
			if (distance <= spotDist){
				bool spotted = SpotCheck(enemies[i].position);
				if (spotted){
					AddTarget(enemies[i]);
				}
			}	
		}	
	}
	public bool SpotCheck(Vector3 target){
		float distance = Vector3.Distance (target, transform.position);
		if (distance < spotDist) {
			SetFOV(fov);

			bool x_side = x_plane.GetSide (target);
			bool z_side = z_plane.GetSide (target);
			//Debug.Log(x_side+ " ; " + z_side + " . ");

			if (x_side && z_side) {
				RaycastHit hit = new RaycastHit();
				Ray ray = new Ray(transform.position, target-transform.position);
				Debug.DrawLine(transform.position, target, Color.cyan);
				if (Physics.Raycast(ray, out hit)){

					if (hit.collider.tag == "Player"){
						//Debug.Log ("Player spotted!");
						Debug.DrawLine(transform.position, hit.point, Color.green);
						return true;
					}
					else{
						Debug.DrawLine(transform.position, hit.point, Color.magenta);
					}
				}
			}
		}
		return false;
	}

	public void SetFOV(float viewAngle){
		Vector3 f_rot = transform.rotation.eulerAngles;
		float normalAngle = 180-viewAngle;
		Quaternion positive = Quaternion.Euler (f_rot.x, f_rot.y+(normalAngle/2), f_rot.z);
		Quaternion negative = Quaternion.Euler (f_rot.x, f_rot.y-(normalAngle/2), f_rot.z);

		x_plane = new Plane (positive * Vector3.forward, transform.position);
		z_plane = new Plane (negative * Vector3.forward, transform.position);
	}

	void DrawVisionCone(int prescision){

		float angle = (180 - Vector3.Angle(x_plane.normal, z_plane.normal));

	
		Vector3[] points = FindPointsOnCircle(spotDist, angle/prescision);
		for (int i = 0; i < prescision; i++) {
			int index = points.Length- (prescision/2) + i;
			int i_plus = index+1;
			//Debug.Log("Index was "+index);
			if (index >= points.Length){
				
				index -= points.Length;
			}
			if (i_plus >= points.Length){
				i_plus -= points.Length;
			}

			if (index < points.Length){
				//Debug.Log("Drawing "+index + " to "+i_plus + " of "+points.Length);
				Debug.DrawLine(transform.position + points[index], transform.position + points[i_plus]);
			}
			if (i == 0 ){
				Debug.DrawRay (transform.position, points[index], Color.red);
			}
			if (i == prescision-1){
				Debug.DrawRay (transform.position, points[i_plus], Color.red);
			}

		}
			
	}

	Vector3[] FindPointsOnCircle(float radius, float angle){
		//find evenly spaced points on circle with radius
		if (angle <= 0){
			angle = 0.1f;
		}
		Vector3[] result = new Vector3[Mathf.RoundToInt(360/angle)];
		float radian = Mathf.Deg2Rad * angle;
		for (int i = 0; i < result.Length; i++) {
			result[i] = transform.rotation * new Vector3(Mathf.Sin(radian * i)*radius, 0, Mathf.Cos(radian * i)*radius);	
		}
		return result;

	}

	public Transform[] GetTargets(){
		return targets;
	}

	public void AddTarget(Transform trg){
		List<Transform> temp = new List<Transform>();
		temp.AddRange(targets);
		temp.Add(trg);
		targets = temp.ToArray();
	}

}
