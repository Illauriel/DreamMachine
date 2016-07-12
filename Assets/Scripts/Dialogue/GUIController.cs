using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUIController : MonoBehaviour {
	//Used to access the GUI elements from GUI Manager

	public Animator animator;

	//public GameObject textbox;
	public GameObject namebox;
	//public GameObject arrow;
	public Button[] choicemenu;
	public Button[] controls;
	public Text text;
	public Text charname;
	public Text zeit;
	public Text geotag;
	public Image fforw;
	public Text floatan;
	public Text backlog;
	public Image backlight;

	public GameObject menuon;
	public GameObject menuoff;
	public GameObject panelclose;
	public GameObject saveframes;

	void Awake(){
		if (animator == null){
			animator = gameObject.GetComponent<Animator>();
		}
	}
	// Use this for initialization
	void Start () {
	
	}

	public void WriteText (string txt) {
		text.text = txt;
	}
	public void ShowCharName (string name) {

	}
	public void SavePanel(){
		animator.SetTrigger("EnableSave");
	}
	public void LoadPanel(){
		animator.SetTrigger("EnableLoad");
	}
	public void CreditsPanel(){
		animator.SetTrigger("EnableCredits");
	}
	public void ClosePanels(){
		animator.SetTrigger("DisablePanels");
	}

	public void ToggleEndline () {
		bool curstate = animator.GetBool("Endline");
		animator.SetBool("Endline", !curstate);
	}

	public void ToggleMainMenu () {
		bool curstate = animator.GetBool("MainMenu");
		animator.SetBool("MainMenu", !curstate);
	}

	public void ToggleUI (){
		bool curstate = animator.GetBool("UIHidden");
		animator.SetBool("UIHidden", !curstate);
	}

	public void ChoiceMenu(string[] options){
		for (int i = 0; i < choicemenu.Length; i++) {
			if (i < options.Length){
				choicemenu[i].gameObject.SetActive(true);
				Text choicetext = choicemenu[i].GetComponentInChildren<Text>();
				choicetext.text = options[i];
				Debug.Log("Enabling menu button!"+i);
			}
			else{
				choicemenu[i].gameObject.SetActive(false);
			}

		}
	}

	public void ExecuteMenu(int id){
		for (int i = 0; i < choicemenu.Length; i++) {
			choicemenu[i].gameObject.SetActive(false);
		}
		LineInterpreter li = GameObject.FindObjectOfType<LineInterpreter>();
		li.ExecuteMenu(id);
	}
}
