using UnityEngine;
using System.Collections;

public class ADVHandler : MonoBehaviour {

	public string back_log;

	GUIManager gui;

	// Use this for initialization
	void Start () {
		gui = FindObjectOfType<GUIManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Say(int id, string text){
		gui.WriteText(text);
	}

	public void Say(string text){
		gui.WriteText(text);
	}
}
