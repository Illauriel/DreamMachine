using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public enum GameStates {Normal, Pause, Combat, Dialogue};
	public GameStates cur_state;
	GameStates prev_state;

	//Things affected by state change
	AgentController[] agents;
	ParticleSystem[] particles;
	Animator[] animators;
	AudioSource[] audiosources;
	Motor[] motors;
	//Combat
	BasicEnemy[] enemies;

	PlayerVoice pVoice;
	Pathfinding pathfinder;

	// Use this for initialization
	void Start () {
		agents = GameObject.FindObjectsOfType<AgentController>();
		particles = LevelInitiator.FindObjectsOfTypeWithTag<ParticleSystem>("AffectedByPause");
		animators = LevelInitiator.FindObjectsOfTypeWithTag<Animator>("AffectedByPause");
		audiosources = LevelInitiator.FindObjectsOfTypeWithTag<AudioSource>("AffectedByPause");
		//motors = LevelInitiator.FindObjectsOfTypeWithTag<Motor>("AffectedByPause");
		motors = GameObject.FindObjectsOfType<Motor>(); //Basically every motor is paused

		enemies = GameObject.FindObjectsOfType<BasicEnemy>();
		pathfinder = GameObject.FindObjectOfType<Pathfinding>();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void TogglePause(){
		if (cur_state != GameStates.Pause){
			prev_state = cur_state;
			Pause();
		}
		else {
			Unpause();
		}
	}
	public void Pause(){
		cur_state = GameStates.Pause;
		for (int i = 0; i < agents.Length; i++) {
			agents[i].Pause();
		}
		for (int i = 0; i < particles.Length; i++) {
			particles[i].Pause(true);
		}

		for (int i = 0; i < motors.Length; i++) {
			motors[i].HaltMotor();
		}
		for (int i = 0; i < animators.Length; i++) {
			animators[i].enabled = false;
		}
		for (int i = 0; i < audiosources.Length; i++) {
			audiosources[i].Pause();
		}
	}
	public void Unpause(){
		cur_state = prev_state;
		for (int i = 0; i < agents.Length; i++) {
			agents[i].Unpause();
		}
		for (int i = 0; i < particles.Length; i++) {
			particles[i].Play(true);
		}
		for (int i = 0; i < motors.Length; i++) {
			motors[i].RestartMotor();
		}
		for (int i = 0; i < animators.Length; i++) {
			animators[i].enabled = true;
		}
		for (int i = 0; i < audiosources.Length; i++) {
			audiosources[i].Play();
		}
	}

	public void StartCombat(){
		prev_state = cur_state;
		cur_state = GameStates.Combat;
		Debug.Log("Game Switching to Combat state!");
		for (int i = 0; i < enemies.Length; i++) {
			enemies[i].state = BasicEnemy.State.Combat;

			pathfinder.ActivateNodes(enemies[i].transform.position);

		}
		Transform player = GameObject.FindGameObjectWithTag("Player").transform;
		pathfinder.ActivateNodes(player.position);
		Debug.Log("Player "+player+" Activates grid!");

		for (int i = 0; i < agents.Length; i++) {
			Vector3 node_center = pathfinder.FindClosestNode(agents[i].transform.position).Pos;
			agents[i].StrafeTo(node_center);
		}

	}
}
