using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	public enum GameStates {Normal, Pause, Combat, Dialogue};
	public GameStates cur_state;
	GameStates prev_state;

	//Things affected by state change
	CharacterSheet[] charsheets;
	AgentController[] agents;
	ParticleSystem[] particles;
	Animator[] animators;
	AudioSource[] audiosources;
	Motor[] motors;
	//Combat
	BasicEnemy[] enemies;

	PlayerVoice pVoice;
	Pathfinding pathfinder;
	CombatManager combat;
	GUIManager gui;
	SquareGrid s_grid;

	// Use this for initialization
	void Start () {
		agents = GameObject.FindObjectsOfType<AgentController>();
		charsheets = LevelInitiator.GetComponentsOnObjects<CharacterSheet>(LevelInitiator.FindGameObjectsOfComponents(agents));
		particles = LevelInitiator.FindObjectsOfTypeWithTag<ParticleSystem>("AffectedByPause");
		animators = LevelInitiator.FindObjectsOfTypeWithTag<Animator>("AffectedByPause");
		audiosources = LevelInitiator.FindObjectsOfTypeWithTag<AudioSource>("AffectedByPause");
		//motors = LevelInitiator.FindObjectsOfTypeWithTag<Motor>("AffectedByPause");
		motors = GameObject.FindObjectsOfType<Motor>(); //Basically every motor is paused

		enemies = GameObject.FindObjectsOfType<BasicEnemy>();
		pathfinder = GameObject.FindObjectOfType<Pathfinding>();
		combat = GameObject.FindObjectOfType<CombatManager>();
		gui = GameObject.FindObjectOfType<GUIManager>();
		s_grid = GameObject.FindObjectOfType<SquareGrid>();

		gui.DisableDialogue();
		gui.EnableNormal();

		s_grid.GridContainer.SetActive(false);
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
			//motors[i].RestartMotor();
			motors[i].Activate();
		}
		for (int i = 0; i < animators.Length; i++) {
			animators[i].enabled = true;
		}
		for (int i = 0; i < audiosources.Length; i++) {
			audiosources[i].Play();
		}
	}

	public void StartCombat(){
		s_grid.GridContainer.SetActive(true);
		prev_state = cur_state;
		cur_state = GameStates.Combat;
		Debug.Log("Game Switching to Combat state!");
		List<CharacterSheet> combatants = new List<CharacterSheet>();
		for (int i = 0; i < enemies.Length; i++) {
			if (enemies[i].state == BasicEnemy.State.Combat){
				//pathfinder.ActivateNodes(enemies[i].transform.position);
				combatants.Add(enemies[i].gameObject.GetComponent<CharacterSheet>());
			}
		}
		Transform player = GameObject.FindGameObjectWithTag("Player").transform;
		//pathfinder.ActivateNodes(player.position);
		combatants.Add(player.GetComponent<CharacterSheet>());
		//Debug.Log("Player "+player+" Activates grid!");

		for (int i = 0; i < agents.Length; i++) {
			Vector3 node_center = s_grid.GetCell(agents[i].transform.position).Position();
			agents[i].StrafeTo(node_center);
		}
		combat.SetParticipants(combatants.ToArray());
	}
	public void StartDialogue(){
		prev_state = cur_state;
		Pause();
		cur_state = GameStates.Dialogue;
		Debug.Log("Game Switching to Dialogue state!");
		gui.EnableDialogue();
		gui.DisableNormal();

	}

	public void EndDialogue(){
		cur_state = prev_state;
		Debug.Log("Dialogue End");
		Unpause();
		gui.DisableDialogue();
		gui.EnableNormal();
	}

}
