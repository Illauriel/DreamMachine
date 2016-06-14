using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node : MonoBehaviour {

	public enum State {Active, Open, Closed, Start, Goal, Path, Blocked};
	public State curState;
	private Vector3 pos;
	public float f_score;
	public float g_score;
	public float h_score;
	public Node parentNode;
	public bool debug;
	public List<Node> connectedNodes;
	bool reveal;
	float rev_timer;
	int cur_depth;

	Renderer myRenderer;
	// Use this for initialization
	void Awake () {
		pos = transform.position;
		connectedNodes = new List<Node>();
		myRenderer = GetComponent<MeshRenderer>();
		gameObject.tag = "Node";
	}

	void Update(){
		if (debug){ //&& parentNode == null){
			/*foreach (Node node in connectedNodes){
				Debug.DrawLine(pos, node.Pos, Color.blue);
				Debug.Log("node exists");
			}*/
		
			if(curState != State.Active && myRenderer != null){
				if (curState == State.Open){
					//myRenderer.material.color = Color.green;
				}
				else if (curState == State.Closed){
					//myRenderer.material.color = new Color(0.2f,1,1,1);
				}
				else if (curState == State.Goal){
					myRenderer.material.color = Color.red;
				}
				else if (curState == State.Path){
					myRenderer.material.color = Color.magenta;
				}
				else {
					Debug.Log(curState);
					myRenderer.material.color = Color.yellow;
				}
			}
		}
		if (reveal){
			if(rev_timer>0){
				rev_timer -= Time.deltaTime;
			}
			else{
				if (!myRenderer.enabled){
					if (curState != State.Blocked){
						myRenderer.enabled = true;
					}
					foreach(Node node in connectedNodes){
						node.Reveal(cur_depth-1);
					}
				}

			}
		}
	}
	public void Initiate(){
		if (curState != State.Blocked){
			curState = State.Active;
		}
		f_score = 0;
		g_score = 0;
		h_score = 0;
		parentNode = null;
	}

	public Vector3 Pos{
		get {return pos;}
		private set {pos = value;}
	}

	public void Reveal(int depth){
		if (depth > 0 ){
			CheckPass();
			reveal = true;
			rev_timer = 0.1f;
			cur_depth = depth;
		}
	}
	public void Colorize(Color col){
		myRenderer.material.color = col;
	}
	void CheckPass(){
		RaycastHit hit = new RaycastHit();
		Ray ray = new Ray(pos+Vector3.up*4, Vector3.down);
		if (Physics.Raycast(ray, out hit)){
			if (hit.collider.gameObject.layer == 9){
				curState = State.Blocked;
			}
		}
	}
}
