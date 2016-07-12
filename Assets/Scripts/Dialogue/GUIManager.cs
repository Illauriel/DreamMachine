using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour {

	public GUIController dialController;



	// Use this for initialization
	void Start () {
	
	}


	public void WriteText(string text){
		dialController.WriteText(text);
	}
	public void ChoiceMenu(string[] options){
		dialController.ChoiceMenu(options);
		Debug.Log("Open Menu!");
	}

	public void EnableDialogue(){
		dialController.gameObject.SetActive(true);
	}
	public void DisableDialogue(){
		dialController.gameObject.SetActive(false);
	}
}
