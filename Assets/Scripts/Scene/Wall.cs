using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Wall : MonoBehaviour {

	MeshRenderer hiding_wall;
	Wall[] neighbors;

	// Use this for initialization
	void Start () {
		if (transform.childCount > 0){
			GameObject child_wall = transform.GetChild(0).gameObject;
			hiding_wall = child_wall.GetComponent<MeshRenderer>();
		}
		else{
			hiding_wall = GetComponent<MeshRenderer>();
		}

		Vector3[] directions = new Vector3[]{Vector3.forward, Vector3.right, Vector3.back, Vector3.left};
		List<Wall> temp_walls = new List<Wall>();
		for (int i = 0; i < 4; i++) {
			
			Ray ray = new Ray(transform.position, directions[i]);
			RaycastHit hit = new RaycastHit();
			int layerMask = 1 << 9;
			if (Physics.Raycast(ray, out hit, 1, layerMask)){
				Wall wall = null;
				if (hit.transform.parent != null){
					wall = hit.collider.gameObject.GetComponentInParent<Wall>();
				}
				else{
					wall = hit.collider.gameObject.GetComponent<Wall>();	
				}

				if (wall != null){
					temp_walls.Add(wall);
				}
				else{
					Debug.Log("Object "+name+" didnt find wal on neighbour "+hit.collider.name);
				}
			}
		}
		neighbors = temp_walls.ToArray();
	}
	
	// Update is called once per frame
	void Update () {
		/*if (!hiding_wall.enabled){
			hiding_wall.enabled = true;
		}*/
	}

	public void HideSection(int depth){
		hiding_wall.enabled = false;
		if (depth>0){
			for (int i = 0; i < neighbors.Length; i++) {
				neighbors[i].HideSection(depth-1);
			}
		}
	}
	public void ShowSection(int depth){
		hiding_wall.enabled = true;
		if (depth>0){
			for (int i = 0; i < neighbors.Length; i++) {
				neighbors[i].ShowSection(depth-1);
			}
		}
	}
}
