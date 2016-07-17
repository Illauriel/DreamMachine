using UnityEngine;
using System.Collections;

public class SpriteAnimator : MonoBehaviour {

	public float[] directions;
	public GameObject[] sprites;

	int cur_index = -1;
	GameObject cursprite;

	//Animator[] animators;
	Animator anim;
	string[] curspriteAnims;
	int[] hashes;
	string curstate;
	// Use this for initialization
	void Start () {
		//animators = new Animator[directions.Length];
		//animators = LevelInitiator.GetComponentsOnObjects<Animator>(sprites);

		curspriteAnims = new string[0];
		ProcessDirection();
	}
	
	// Update is called once per frame
	void Update () {
		
		ProcessDirection();
		if (anim != null){
			AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
			for (int i = 0; i < curspriteAnims.Length; i++) {
				if (state.shortNameHash == hashes[i]){
					curstate = curspriteAnims[i];
				}
				//Debug.Log(state.shortNameHash + " "+ state.fullPathHash +" " +state.tagHash);
			}

		}
	}
	void LateUpdate(){
		if(cursprite != null){
			cursprite.transform.position = transform.position-Vector3.up;
			cursprite.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
		}
	}


	public void SetFloat(string f_name, float value){
		anim.SetFloat(f_name, value);
	}
	public void SetBool(string b_name, bool value){
		anim.SetBool(b_name, value);
		Debug.Log(gameObject.name + " sets bool "+b_name);
	}

	public void GetAnims(){
		anim = cursprite.GetComponent<Animator>();
		if (anim != null && anim.runtimeAnimatorController != null){
			curspriteAnims = new string[anim.runtimeAnimatorController.animationClips.Length];
			hashes = new int[curspriteAnims.Length];
			for (int i = 0; i < anim.runtimeAnimatorController.animationClips.Length; i++) {
				curspriteAnims[i] = anim.runtimeAnimatorController.animationClips[i].name;
				hashes[i] = Animator.StringToHash(curspriteAnims[i]);
			//	Debug.Log(curspriteAnims[i] + " hash " + hashes[i]);
			}
		}
			
	}

	void ProcessDirection(){
		if (directions.Length > 1 && sprites.Length > 0){
			float y_angle = transform.eulerAngles.y;
			int dir_index = -1;
			if (y_angle > directions[directions.Length-1] || y_angle <= directions[0]){
				dir_index = 0;
				//Debug.Log(0+" Angle "+y_angle+" falls between "+directions[directions.Length-1] + " and "+directions[0]);
			}
			else{
				for (int i = 1; i < directions.Length; i++) {
					float min_angle = directions[i-1];
					float max_angle = directions[i];
					if (y_angle > min_angle && y_angle <= max_angle){
						//Debug.Log(i+" Angle "+y_angle+" falls between "+min_angle + " and "+max_angle);
						dir_index = i;
					}
					else{
						//Debug.Log(i + "The rotation of "+y_angle+" degrees missed range "+ min_angle+"-"+max_angle);
					}
				}
			}
			if (dir_index == -1){
				Debug.LogError("The rotation of "+y_angle+" degrees is a failure");
			}
			if (dir_index != cur_index){
				double anim_phase = 0;
				if (anim != null){
					anim_phase = anim.GetTime();
				}
				cur_index = dir_index;

				Destroy(cursprite);
				if (sprites[cur_index] != null){
					Debug.Log("Instatioating object "+ cur_index + " "+ gameObject.name);
					cursprite = (GameObject) Instantiate(sprites[cur_index], transform.position, Quaternion.identity);
					anim = cursprite.GetComponent<Animator>();
					if (anim != null){
						GetAnims();
						anim.Play(curstate);
						anim.SetTime(anim_phase);
						//cursprite.transform.parent = transform;
					}	
				}
				else {
					Debug.Log("Sprite "+cur_index+" is unassigned!");
				}
			}
		}

		else{
			Debug.LogError("The sprites on "+name+ " are broken!");
		}
	}


	void OnGUI(){
		
		/*for (int i = 0; i < curspriteAnims.Length; i++) {
			if (GUI.Button(new Rect(10,10+20*i, 100, 20), curspriteAnims[i])){
				anim.Play(curspriteAnims[i]);
			}
		}
		GUI.color = Color.blue;
		GUI.Label(new Rect(120,10, 500, 100), "Currently playing: "+curstate);
		GUI.color = Color.white;
	}*/
	}
}
