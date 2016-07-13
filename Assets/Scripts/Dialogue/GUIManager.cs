using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour {

	public GUIController dialController;
	public GUIController playController;
	public LevelResources data;



	// Use this for initialization
	void Start () {
	
	}


	public void WriteText(string text){
		dialController.WriteText(text);
	}
	public void ShowName(int id){
		dialController.ShowCharName(data.characters[id].charname);
		dialController.text.color = data.characters[id].charcolor;
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

	public void EnableNormal(){
		playController.gameObject.SetActive(true);
	}
	public void DisableNormal(){
		playController.gameObject.SetActive(false);
	}

}
