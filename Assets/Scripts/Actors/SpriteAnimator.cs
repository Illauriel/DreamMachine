using UnityEngine;
using System.Collections;

public class SpriteAnimator : MonoBehaviour {

	public float[] directions;
	public GameObject[] sprites;

	int cur_index = -1;
	GameObject cursprite;

	//Animator[] animators;
	Animator anim;

	// Use this for initialization
	void Start () {
		//animators = new Animator[directions.Length];
		//animators = LevelInitiator.GetComponentsOnObjects<Animator>(sprites);
		ProcessDirection();
	}
	
	// Update is called once per frame
	void Update () {
		
		ProcessDirection();
	}
	void LateUpdate(){
		cursprite.transform.position = transform.position;
		cursprite.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
	}

	public void SetFloat(string f_name, float value){
		//animators[cur_index].SetFloat(f_name, value);
	}
	public void SetBool(string b_name, bool value){
		//animators[cur_index].SetBool(b_name, value);
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
				//Debug.Log("Instatioating object "+ cur_index);
				cursprite = (GameObject) Instantiate(sprites[cur_index], transform.position, Quaternion.identity);
				anim = cursprite.GetComponent<Animator>();
				anim.SetTime(anim_phase);
				//cursprite.transform.parent = transform;
			}
		}

		else{
			Debug.LogError("The sprites on "+name+ " are broken!");
		}
	}
}
