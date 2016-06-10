using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public Transform target;
	public float distance = 10;
	public float max_dist;
	public float long_angle = 15;
	public float lat_angle = 90;
	public float min_lat;


	// Use this for initialization
	void Start () {
		//float angle
		//Debug.(new Vector3(sin()));
	}

	void Update(){
		if (Input.GetAxis("Mouse ScrollWheel") != 0){
			distance -= Input.GetAxis("Mouse ScrollWheel");
		}
		Vector2 input_axes = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

		ProbeWall(transform.position, Vector3.forward-Vector3.down);
		//ProbeWall(Vector3.back*3);
		ProbeWall(transform.position, Vector3.left-Vector3.down);
		//ProbeWall(Vector3.right*3);

	}
	// Update is called once per frame
	void LateUpdate () {
		if (distance < 6){
			distance = 6;
		}
		else if (distance > max_dist){
			distance = max_dist;
		}
		if (lat_angle > 89.9f){
			lat_angle = 89.9f;
		}
		else if (lat_angle < min_lat){
			lat_angle = min_lat;
		}
		if (long_angle > 360){
			long_angle -= 360;
		}
		else if (long_angle < 0){
			long_angle += 360;
		}
		if (target != null){
			float h_angle = Mathf.Deg2Rad * long_angle;
			float v_angle = Mathf.Deg2Rad * lat_angle;
			float x = target.position.x + distance * Mathf.Cos(h_angle) * Mathf.Cos(v_angle);
			float y = target.position.y + distance * Mathf.Sin(v_angle);
			float z = target.position.z + distance * Mathf.Sin(h_angle) * Mathf.Cos(v_angle);
			
			transform.position = new Vector3(x, y, z);

			transform.LookAt(target.position);
		}
		else {
			//Debug.LogWarning ("No Target for the camera!");
		}
	}

	public void ProbeWall(Vector3 start, Vector3 offset){
		
		Vector3 player_foot = target.position-offset;
		Ray ray = new Ray(start, (player_foot - start));
		float range = Vector3.Distance(start, player_foot);
		//ProbeWall(ray);
		RaycastHit hit = new RaycastHit();
		int layerMask = 1 << 9;
		Debug.DrawLine(start, (player_foot - start));
		if(Physics.Raycast(ray, out hit, range, layerMask)){
			//Wall wall = hit.collider.gameObject.GetComponentInParent<Wall>();
			WallSorting wall = hit.collider.gameObject.GetComponentInParent<WallSorting>();;
			if (wall != null){
				//wall.HideSection(3);
				wall.ToForeground();
				wall.elevate = true;
				//ProbeWall(ray);
			}
			else{
				//Debug.LogError("Wall "+hit.collider.name+" has no Wall script!");
			}
		}
	}
	public void ProbeWall(Ray ray){
		RaycastHit hit = new RaycastHit();
		int layerMask = 1 << 9;
		Debug.DrawLine(ray.origin, ray.direction);
		if(Physics.Raycast(ray, out hit, 100, layerMask)){
			//Wall wall = hit.collider.gameObject.GetComponentInParent<Wall>();
			WallSorting wall = hit.collider.gameObject.GetComponentInParent<WallSorting>();;
			if (wall != null){
				//wall.HideSection(3);
				wall.ToForeground();
				wall.elevate = true;
				//ProbeWall(ray);
			}
			else{
				Debug.LogError("Wall "+hit.collider.name+" has no Wall script!");
			}
		}
	}


}
