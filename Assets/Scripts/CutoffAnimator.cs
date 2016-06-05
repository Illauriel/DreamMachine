using UnityEngine;
using System.Collections;

public class CutoffAnimator : MonoBehaviour {

	Renderer myRenderer;
	public float cutoff;
	public float time;
	float timer;
	public bool animate;

	// Use this for initialization
	void Start () {
		myRenderer = GetComponent<MeshRenderer>();
		timer = time;
		cutoff = 1;
	}
	
	// Update is called once per frame
	void Update () {
		if (animate){
			timer -= Time.deltaTime;
			cutoff = timer/time;
			if (cutoff < 0.01f){
				cutoff = 0.05f;
			}

		}
		else {
			if (timer < time){
				timer = time;
				cutoff = 1;
			}
		}
		if (myRenderer.material.GetFloat("_Cutoff") != cutoff){
			myRenderer.material.SetFloat("_Cutoff", cutoff);
		}
	}
}
