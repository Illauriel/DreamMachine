using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pathfinding: MonoBehaviour {

	public bool is_grid;
	public int step;
	//Node start_node;
	//Node end_node;

	public  Vector3[] pass;

	public Node[] allNodes;
	//public GameObject[] nodes;
	// Use this for initialization
	void Awake(){
		RenewNodesList();
	}
	void Start () {
		if (allNodes.Length == 0){
			RenewNodesList();
		}


		/*start_node = allNodes[0];
		end_node = allNodes[allNodes.Length-1];

		start_node.gameObject.name = "!!!Start!!!";
		end_node.gameObject.name = "++End++";

		start_node.curState = Node.State.Start;
		end_node.curState = Node.State.Goal;*/
		if (is_grid){
			EstablishRadialConnections(1.1f);
		}
		else{
			EstablishConnections(6);
		}
		pass = new Vector3[0];
		step = 0;
	}

	void Update(){
		if (pass.Length > 0){
			for (int i = 0; i < pass.Length-1; i++) {
				Debug.DrawLine(pass[i], pass[i+1], new Color(1, 0.5f, 0, 1));	
			}
		}
	}

	public Vector3[] FindPath(Node start, Node end){
		if (start != null && end != null){
			//Debug.Log("Finding path from "+start+" to "+end);
			pass = new Vector3[0];
			step = 0;
			for (int i = 0; i < allNodes.Length; i++) {
				allNodes[i].Initiate();
			}
			//start_node = start;
			//end_node = end;
			start.curState = Node.State.Start;
			end.curState = Node.State.Goal;


			ConnectStart(start, end);
			step++;
			for (int i = 0; i < allNodes.Length; i++) {
				ProcessOpenNodes(end);
				if (pass.Length > 0){
					break;
				}
				step++;
			}
			//check total path length
			float total_dist = 0;
			for (int i = 0; i < pass.Length-1; i++) {
				total_dist += Vector3.Distance(pass[i], pass[i+1]);
			}
		//	Debug.Log("Pass length is " + total_dist);
			return pass;
		}
		else {
			if (start == null){
				Debug.LogWarning("Start Node is null!");
				start = end;
			}
			if (end == null){
				Debug.LogWarning("End Node is null!");
				end = start;
			}
			pass = new Vector3[1];
			pass[0] = start.Pos;
			return pass;
		}
	}
	public Vector3[] FindPath(Vector3 start, Vector3 end){
		Node startNode = FindClosestNode(start);
		Node endNode = FindClosestNode(end);
		Vector3[] result = FindPath(startNode, endNode);
		return result;
		
	}
	private void ConnectStart (Node start, Node end){
		//foreach (Node x in start_node.connectedNodes){
		//for (int i = 0; i < allNodes.Length; i++) {
		Node[] connections = start.connectedNodes.ToArray(); 
		for (int i = 0; i < connections.Length; i++) {
			if(connections[i].curState != Node.State.Blocked){
				float distToStart = Vector3.Distance(start.Pos, connections[i].Pos);
				float distToGoal = Vector3.Distance(connections[i].Pos, end.Pos);

				connections[i].parentNode = start;
				connections[i].curState = Node.State.Open;
				connections[i].g_score = distToStart;
				connections[i].h_score = distToGoal;
				connections[i].f_score = distToStart + distToGoal;
			}
			//
		}
		//Debug.Log("Starting Node "+start_node + " connected");
	}

	/*public void MakeStep(){
		if (step == 0){
			ConnectStart();
			step++;
		}

		else{
			ProcessOpenNodes(end);
			step++;
		}
	}*/

	private void ProcessOpenNodes(Node end){
		//List<Node> open_nodes = new List<Node>();
		Node best_node = null;
		float best_score = Mathf.Infinity;
		for (int i = 0; i < allNodes.Length; i++) {
			if (allNodes[i].curState == Node.State.Open){
				if(allNodes[i].f_score < best_score){
					best_node = allNodes[i];
					best_score = allNodes[i].f_score;
				}
				//open_nodes.Add(allNodes[i]);

			}
		}
		if (best_node != null){
			for (int i = 0; i < best_node.connectedNodes.Count; i++) {
				BestParent(best_node.connectedNodes[i], best_node, end);
			}
			best_node.curState = Node.State.Closed;
		}
		/*if (open_nodes.Count > 0){
			for (int i = 0; i < open_nodes.Count; i++) {
				Debug.Log(open_nodes[i].name + " is open!");
				for (int j = 0; j < open_nodes[i].connectedNodes.Count; j++) {
					BestParent(open_nodes[i].connectedNodes[j], open_nodes[i]);
					if (open_nodes[i].connectedNodes[j].curState == Node.State.Goal){
						Debug.LogWarning("End Found!");
					}
				}
				open_nodes[i].curState = Node.State.Closed;
			}
		}*/
		else{
			//Debug.LogWarning("All of the possible paths explored!");
			pass = TruePath(end);
		}
	}

	void BestParent(Node node, Node p_parent, Node end){
		//if (node.curState == Node.State.Active || node.curState == Node.State.Open){
		if (node.curState != Node.State.Start && node.curState != Node.State.Closed && node.curState != Node.State.Blocked){
			float distToStart = p_parent.g_score + Vector3.Distance(p_parent.Pos, node.Pos);
			float distToGoal = Vector3.Distance(node.Pos, end.Pos);
			float f_score = distToGoal + distToStart;
			if (node.f_score == 0 || node.f_score > f_score){
				node.g_score = distToStart;
				node.h_score = distToGoal;
				node.f_score = f_score;
				node.parentNode = p_parent;
				//Debug.Log("BestParent for node " + node.name + " is " + p_parent.name );
				if (node.curState != Node.State.Goal){
					node.curState = Node.State.Open;
				}
			}
		}
	}
	
	private Vector3[] TruePath(Node end){
		List<Node> bestPath = new List<Node>();
		Node curNode = end;
		//float lowestScore = -1;
		for (int i = 0; i < allNodes.Length; i++) {

			bestPath.Add(curNode);
			if (curNode.curState != Node.State.Start){
				Node curparent = curNode.parentNode;
				//bestPath.Add(curparent);
				if (curparent != null){
					curNode = curparent;
				}
				else{
					//Debug.LogWarning("Achtung! Item "+curNode.gameObject.name+" Has no parents but is on optimal path!");
				}
			}
			else{

				break;
			}
		}
		bestPath.Reverse();
		Vector3[] result = new Vector3[bestPath.Count];
		for (int i = 0; i < result.Length; i++) {
			result[i] = bestPath[i].Pos;
		}

		return result;
	}
	/*private bool NodeReachable(){
		Vector3 start = start_node.Pos;
		Vector3 end = end_node.Pos;
		float goalDistance = Vector3.Distance(start, end);
		
		if (!Physics.Raycast(start, end - start, goalDistance)){
			return true;
		}
		else {
			return false;
		}
		
	}*/

	public void EstablishConnections(float radius){
		//Debug.Log("BuildingConnections");
		foreach (Node node1 in allNodes){
			Vector3 pos1 = node1.Pos;

			foreach (Node node2 in allNodes){
				Vector3 pos2 = node2.Pos;
				//Debug.Log(pos1 + " "+pos2);
				float distance = Vector3.Distance(pos1, pos2);
				RaycastHit hit = new RaycastHit();
				if (distance != 0 && distance < radius && !Physics.Raycast(pos1, pos2 - pos1, out hit, distance)){
					//node1.AddConnectedNode(node2);
					node1.connectedNodes.Add(node2);
				}
				else{
					if (hit.transform != null){
						//Debug.LogWarning("Hitshit "+hit.transform.name);
					}
					
				}
			}
			if (node1.connectedNodes.Count == 0) {
				Debug.Log ("WARNING = НИРАБОТАЕТ!");
			}
		}
	}
	public void EstablishRadialConnections(float radius){
		foreach (Node node1 in allNodes){
			Vector3 pos1 = node1.Pos;
			
			foreach (Node node2 in allNodes){
				Vector3 pos2 = node2.Pos;
				//Debug.Log(pos1 + " "+pos2);
				float distance = Vector3.Distance(pos1, pos2);
				if (distance != 0 && distance <= radius){
					/*Debug.Log(node1);
					Debug.Log(node1.connectedNodes);
					Debug.Log(node2);
					Debug.Log(distance);*/
					node1.connectedNodes.Add(node2);
				}
			}
		}
	}
	public void ClearConnections(){
		foreach (Node node1 in allNodes){
			//node1.ClearConnectedNodes();
			node1.connectedNodes.Clear();
		}
	}
	public Node FindNodeInRadius(float radius, Vector3 point){
		List<Node> nodes_in_rad = new List<Node>();
		for (int i = 0; i < allNodes.Length; i++) {
			if (Vector3.Distance(point, allNodes[i].transform.position) < radius) {
				nodes_in_rad.Add (allNodes [i]);
			}
		}
		if (nodes_in_rad.Count > 0) {
			return nodes_in_rad [Random.Range (0, nodes_in_rad.Count - 1)];

		} 
		else {
			Debug.LogError ("null nodes found in radius");
			return null;

		}
	}
	public Node FindClosestNode (Vector3 point){
		Node closest = null;
		float close_dist = Mathf.Infinity;

		for (int i = 0; i < allNodes.Length; i++) {
			float dist = Vector3.Distance(point, allNodes[i].transform.position);
			if (dist < close_dist) {
				RaycastHit hit = new RaycastHit();
				if (Physics.Raycast(point, allNodes[i].transform.position - point, out hit, dist)){
					// Nope it's not in direct reach;
					//Debug.Log (allNodes[i].name + "  "+ hit.collider.name + " " + dist);
				}
				else{
					closest = allNodes[i];
					close_dist = dist;
				}
			}
		}
		return closest;
	}

	public float GetPathDistance(Vector3 point, Vector3 dest){
		Node new_start = FindClosestNode(point);
		Node new_end = FindClosestNode(dest);
		Vector3[] pathnodes = FindPath(new_start, new_end);
		float dist = Vector3.Distance(point, pathnodes[0]);
		for (int i = 0; i < pathnodes.Length-1; i++) {
			dist += Vector3.Distance(pathnodes[i], pathnodes[i+1]);
		}


		return dist;
	}

	public void ActivateNodes(Vector3 point){
		Node first = FindClosestNode(point);
		if (first != null){
			first.Reveal(10);
		}


		//AgentController[] 
		//foreach()
	}

	void RenewNodesList(){
		GameObject[] nodes = GameObject.FindGameObjectsWithTag("Node");
		allNodes = new Node[nodes.Length];
		for (int i = 0; i < nodes.Length; i++) {
			allNodes[i] = nodes[i].GetComponent<Node>();
		}
	}
}
