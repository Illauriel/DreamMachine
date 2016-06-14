using UnityEngine;
using System.Collections;

public class WallSorting : MonoBehaviour {

	public Transform sprite;
	public WallSorting[] linkedWalls;
	Transform foreground;
	Transform background;
	//int index;
	public bool elevate;
	// Use this for initialization
	void Start () {
		//gameObject.layer = LayerMask.NameToLayer("Wall");
		foreground = GameObject.Find("Foreground").transform;
		background = GameObject.Find("Background").transform;
	}
	void Update(){
		//elevate = false;
	}
	// Update is called once per frame
	void LateUpdate () {
		/*if (!elevate ){
			for (int i = 0; i < linkedWalls.Length; i++) {
				Debug.Log(name+" Checking");
				if (linkedWalls[i].elevate){
					elevate = true;
					Debug.Log("Set tru");
				}
			}

		}
		if (!elevate && sprite.parent != background){

			ToBackground();
		}*/
	}
	void OnTriggerEnter(Collider col){
		//Debug.Log("SmthEnter");
		if (col.gameObject.tag == "Player"){
			Debug.Log("Player Enter");
			ToForeground();

		}
	}
	void OnTriggerStay(Collider col){
		if (col.gameObject.tag == "Player"){
			if (sprite.parent != foreground){
				ToForeground();
			}
		}
	}
	void OnTriggerExit(Collider col){
		if (col.gameObject.tag == "Player"){
			ToBackground();
		}
	}
	/// <summary>
	/// Moves the attached sprite to Foreground canvas.
	/// </summary>
	public void ToForeground(){
		if (sprite.parent != foreground){
			//Debug.Log(name+" Foregrounding");
			//Transform[] siblingsToMove = new Transform[sprite.parent.childCount-index];
			ParentTo(foreground);
			//sprite.SetSiblingIndex(index);
			DisableWall();
		}
	}

	public void ToBackground(){
		Debug.Log(name+" Backgrounding");
		ParentTo(background);
		EnableWall();
		//sprite.SetSiblingIndex(index);
	}

	void ParentTo(Transform parent){
		int index = sprite.GetSiblingIndex();
		int siblingsToMove = sprite.parent.childCount-index;
		for (int i = 0; i < siblingsToMove; i++) {
			//sprite.SetParent(foreground, false);	
			Debug.Log("Parenting "+sprite.parent.GetChild(index+i).name + " to "+parent);
			sprite.parent.GetChild(index+i).SetParent(parent, false);
		}

	}

	void DisableWall(){
		for (int i = 0; i < linkedWalls.Length; i++) {
			Wall thiswall = linkedWalls[i].GetComponent<Wall>();
			thiswall.HideSection(0);
		}
	}
	void EnableWall(){
		for (int i = 0; i < linkedWalls.Length; i++) {
			Wall thiswall = linkedWalls[i].GetComponent<Wall>();
			thiswall.ShowSection(0);
		}
	}
}
