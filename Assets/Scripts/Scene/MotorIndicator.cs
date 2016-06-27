using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class MotorIndicator : MonoBehaviour {

	public Motor motor;
	public Image img;

	public Sprite[] sprites;
	//public GameObject[] stateObjs;
	//public GameObject startObj;
	/*public GameObject runningObj;
	public GameObject haltObj;
	public GameObject stopObj;
	public GameObject endObj;*/



	// Use this for initialization
	void Start () {
		// = new GameObject[5];
	}
	
	// Update is called once per frame
	void Update () {
		if (img.sprite != sprites[(int) motor.cur_state]){
			for (int i = 0; i < sprites.Length; i++) {
				if (i == (int) motor.cur_state){
					//stateObjs[i].SetActive(true);
					img.sprite = sprites[i];
				}

			}
		}
	}
}
