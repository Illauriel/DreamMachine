using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Priority_Queue;

public class AStar : Pathfinding {

	public SquareGrid s_grid;
	bool step;

	//List <int> frontier;
	SimplePriorityQueue <int> frontier;

	int[] came_from;
	float[] cost_so_far;


	int start;
	int destination;

	int searchcount;


	public void StartPath(){
		StartPath(start);
	}

	void StartPath(int start_id){
		frontier = new SimplePriorityQueue<int>();
		frontier.Enqueue(start_id, 0);
		InitGridData();
		came_from[start] = -2;
		cost_so_far[start] = 0;
		searchcount = 0;
	}

	void StartPath(Vector3 start_vec){
		GridCell start_cell = s_grid.GetCell(start_vec);
		int start_id = s_grid.GetCellId(start_cell);
		Debug.Log("Clickan cell #"+start_id+" "+start_cell.x+":"+start_cell.y+":"+start_cell.z+" @ "+start_vec);
		StartPath(start_id);
	}

	public void AdvanceFrontier(){
		if (frontier.Count != 0 && !frontier.Contains(destination)){
			int cycles = frontier.Count;
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
		int cell_id = frontier.Dequeue();
		int[] new_ids = OddSearch(cell_id);

		for (int i = 0; i < 4; i++) {
			int neighbor_id = s_grid.neighbor_ids[cell_id, new_ids[i]];
			if (neighbor_id != -1){
				float new_cost = cost_so_far[cell_id] + s_grid.GetCost(neighbor_id);

				//If the visited tile list doesn't contain this neighbor
				if (came_from[neighbor_id] == -1 || new_cost < cost_so_far[neighbor_id]){
					cost_so_far[neighbor_id] = new_cost;
					//Add it to frontier
					float priority = new_cost + ManhattanDist(neighbor_id, destination);
					frontier.Enqueue(neighbor_id, priority);
					//Mark step 
					searchcount++;
					//s_grid.gui.SetText(neighbor_id, ""+priority);
					//also add it to visited

					came_from[neighbor_id] = cell_id;
					//Debug.Log("Adding "+ s_grid.all_cells[neighbor_id].x+":"+ s_grid.all_cells[neighbor_id].y + " at ht "+s_grid.all_cells[neighbor_id].z);
				}
			}
		}
	}
	int ManhattanDist(int a, int b){
		int x_diff = Mathf.Abs(s_grid.all_cells[a].x - s_grid.all_cells[b].x);
		int y_diff = Mathf.Abs(s_grid.all_cells[a].y - s_grid.all_cells[b].y);
		return x_diff+y_diff;

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

	int[] DistPriority(int id){
		int[] ids = s_grid.GetNeighbors(id);
		int[] result = new int[4];
		float[] dists = new float[4];

		for (int i = 0; i < 4; i++) {
			dists[i] = s_grid.Distance(ids[i], start);
			result[i] = i;
		}

		for (int i = 0; i < 4; i++) {
			int min = -1;
			float mindist = Mathf.Infinity;
			for (int j = i; j < 4; j++) {
				if (dists[j] < mindist){
					mindist = dists[j];
					min = j;
				}
			}

			if (min != -1){
				float tempdist = dists[i];
				int temp = result[i];
				dists[i] = dists[min];
				result[i] = result[min];
				dists[min] = tempdist;
				result[min] = temp;
			}

		}

		return result;
	}

	public int[] GetFrontier(){
		return frontier.ToArray();
	}

	public int[] GetCameFrom(){
		return came_from;
	}
	public void SetDestination(int dest_id){
		destination = dest_id;
	}
	public void SetDestination(Vector3 dest_point){
		GridCell dest_cell = s_grid.GetCell(dest_point);
		int dest_id = s_grid.GetCellId(dest_cell);
		destination = dest_id;
	}
	public int GetDestination(){
		return destination;
	
	}
	public void SetStart(int start_id){
		start = start_id;
	}
	public void SetStart(Vector3 start_point){
		GridCell start_cell = s_grid.GetCell(start_point);
		int start_id = s_grid.GetCellId(start_cell);
		start = start_id;
	}
	public int GetStart(){
		return start;
	}

	public Vector3[] FindPath(){
		StartPath();
		int[] path_ind = FindPath(true);
		Vector3[] result = new Vector3[path_ind.Length];
		for (int i = 0; i < path_ind.Length; i++) {
			result[i] = s_grid.all_cells[path_ind[i]].Position();
		}
		return result;
	}
	public int[] FindPath(bool yes){
		//Debug.Log(frontier);
		int[] result = new int[0];
		for (int i = 0; i < s_grid.all_cells.Length; i++) {
			if (!frontier.Contains(destination)){
				SearchLoop();//frontier[0]);
			}
			else{
				result = RestorePath();
				break;
			}
		}
		return result;
	}

	public int[] RestorePath(){
		List<int> path = new List<int>();
		if (came_from[destination] != -1){
			//int current = destination;
			path.Add(destination);
			//int front_index = frontier.ToArray()[frontier.Count-1];
			int next = came_from[destination];
			for (int i = 0; i < s_grid.all_cells.Length; i++) {
				if (next != start){
					path.Add(next);
					//int vis_index = visited.IndexOf(next);
					next = came_from[next];
				}
				else {
					break;
				}
			}
		}
		return path.ToArray();
	}

	void InitGridData(){
		came_from = new int[s_grid.all_cells.Length];
		cost_so_far = new float[s_grid.all_cells.Length];
		for (int i = 0; i < came_from.Length; i++) {
			came_from[i] = -1;
			cost_so_far[i] = Mathf.Infinity;

		}
	}
}
