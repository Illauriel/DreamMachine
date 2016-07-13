using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InfluenceMaps : MonoBehaviour {

	public GameObject marker;
	GameObject[] markers;
	SquareGrid s_grid;
	Queue<int> frontier;
	List <int> visited;
	// Use this for initialization
	void Start () {
		s_grid = GameObject.FindObjectOfType<SquareGrid>();
		visited = new List<int>();
		markers = new GameObject[s_grid.all_cells.Length];
		for (int i = 0; i < markers.Length; i++) {
			markers[i] = (GameObject) Instantiate(marker, s_grid.all_cells[i].Position(), marker.transform.rotation);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	/// <summary>
	/// Builds the influence map.
	/// </summary>
	/// <returns>The map.</returns>
	/// <param name="side_index">Index of side for which we calculate map. Here 0 is allies and 1 is enemies.</param>
	public float[] GetMap(int side_index){
		float[] result = new float[s_grid.all_cells.Length];
		int ally_code = 3;
		int enemy_code = 4;
		if (side_index == 1){
			ally_code = 4;
			enemy_code = 3;
		}
		for (int i = 0; i < s_grid.cell_status.Length; i++) {
			if (s_grid.cell_status[i] == enemy_code){
				result[i] = 0;
				float[] fill = FloodFill(i, 10);
				for (int j = 0; j < fill.Length; j++) {
					result[j] += fill[j];
				}
			}
			/*else if (s_grid.cell_status[i] == 0 || s_grid.cell_status[i] == 2 ){
				result[i] = 0.1f;
			}
			else {
				result[i] = 0;
			}*/
		}
			
		for (int i = 0; i < result.Length; i++) {
			
			markers[i].GetComponent<MeshRenderer>().material.color = new Color(result[i], 0, 0, 1);
		}
		return result;
	}

	float CellValue(int cell_id){
		if (s_grid.cell_status[cell_id] == 0 || s_grid.cell_status[cell_id] == 2){
			return 0.5f;
		}
		else{
			return 0;
		}
	}


	int ManhattanDist(int a, int b){
		int x_diff = Mathf.Abs(s_grid.all_cells[a].x - s_grid.all_cells[b].x);
		int y_diff = Mathf.Abs(s_grid.all_cells[a].y - s_grid.all_cells[b].y);
		return x_diff+y_diff;

	}

	float[] FloodFill(int start, int radius){
		float[] result = new float[s_grid.cell_status.Length];
		frontier = new Queue<int>();
		visited.Clear();
		frontier.Enqueue(start);
		for (int i = 0; i < radius; i++) {
			AdvanceFrontier();
			int[] front = frontier.ToArray();
			foreach(int x in frontier){
				result[x] = 1 - 0.1f * i;
			}
		}
		return result;
	}
	public int FindDestination(int start, int radius, float[] map){
		int result = -1;
		float best = 0;
		frontier = new Queue<int>();
		visited.Clear();
		frontier.Enqueue(start);
		for (int i = 0; i < radius; i++) {
			AdvanceFrontier();
			int[] front = frontier.ToArray();
			foreach(int x in frontier){
				if (map[x] > best){
					result = x;
					best = map[x];
				}
			}
		}
		return result;
	}

	//This is part of Flood Fill
	public void AdvanceFrontier(){
		if (frontier.Count != 0){
			int cycles = frontier.Count;
			//For each cell in current frontier
			for (int i = 0; i < cycles; i++) {
				//check each neighbor
				SearchLoop();
			}
		}
		else{
			Debug.Log("Search End");
		}
	}

	public void SearchLoop(){
		//Remove tile from Frontier
		//frontier.Remove(cell_id);
		int cell_id = frontier.Dequeue();
		visited.Add(cell_id);
		int[] new_ids = OddSearch(cell_id);

		for (int i = 0; i < 4; i++) {
			//Debug.Log(i + " Looking at neighbr " + dist_ids[i]);
			int neighbor_id = s_grid.neighbor_ids[cell_id, new_ids[i]];
			if (neighbor_id != -1){
				//If the visited tile list doesn't contain this neighbor
				if (!visited.Contains(neighbor_id) && s_grid.cell_status[neighbor_id] != 1){
					//Add it to frontier
					frontier.Enqueue(neighbor_id);

				}
			}
		}
	}

	int[] OddSearch(int id){
		int[] result = new int[4];
		for (int i = 0; i < 4; i++) {
			if (id % 2 == 0){
				result[i] = i;
			}
			else {
				result[i] = 3-i;
			}
		}
		return result;
	}


}
