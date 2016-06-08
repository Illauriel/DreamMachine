﻿using UnityEngine;
using System.Collections;

public class BasicEnemy : MonoBehaviour {

	//state machine 
	public enum State {Patrol, Interested, Hunt, Combat};
	public State state; 
	public BasicEnemy[] comrades;

	//Turn Based elements
	CharacterSheet[] enemies;
	CharacterSheet charsheet;
	CharacterSheet target;
	bool myturn;
	bool hasSwiftAction;
	bool hasAttackAction;
	bool hasMovePoints;

	//my components
	AIPerception percept;
	AgentController aController;

	GameController gc;
	CombatManager combat;

	// Use this for initialization
	void Start () {
		percept = GetComponent<AIPerception>();
		aController = GetComponent<AgentController>();
		charsheet = GetComponent<CharacterSheet>();
		//And start following patrol nodes;
		PatrolStateTransition();

		gc = GameObject.FindObjectOfType<GameController>();
		combat = GameObject.FindObjectOfType<CombatManager>();
		charsheet.aiControlled = true;
		enemies = new CharacterSheet[0];
	}
	
	// Update is called once per frame
	void Update () {
		StateSolver();
	}

	void AIPatrol(){
		aController.MoveInPath(aController.walkingSpeed);

	}

	void AICombat(){
		if (!aController.destReached){
			aController.MoveInPath();
		}
		else{
			if (aController.MovePoints == 0){
				hasMovePoints = false;
			}
				
			Decision();

		}
	}

	void StateSolver(){
		if (state == State.Patrol){
			AIPatrol();
		}
		if (state == State.Combat && myturn){
			AICombat();
		}

		if (percept.GetTargets().Length > 0 && state != State.Combat){
			CombatStateTransition();
		}
	}

	void PatrolStateTransition(){
		state = State.Patrol;
		aController.StartPatrol();
	}

	public void CombatStateTransition(){
		state = State.Combat;
		//Fill in enemy
		if (enemies.Length == 0){
			Transform[] targets = percept.GetTargets();
			enemies = new CharacterSheet[targets.Length];
			for (int i = 0; i < targets.Length; i++) {
				enemies[i] = targets[i].GetComponent<CharacterSheet>();
			}
		}
		for (int i = 0; i < comrades.Length; i++) {

			if (comrades[i].state != State.Combat){
				comrades[i].GroupAwareness(enemies);
				comrades[i].CombatStateTransition();

			}
		}
		//stop previous movements
		aController.CleanPath();
		gc.StartCombat();
	}

	public void StartCombatTurn(){
		myturn = true;
		hasMovePoints = true;
		hasSwiftAction = true;
		hasAttackAction = true;
		aController.MovePoints = charsheet.speed;
		Decision();
	}

	void Decision(){
		//if no target - find target
		if (target == null){
			SelectTarget();

		}
		//If adjacent to target
		if (hasAttackAction && Vector3.Distance(transform.position, target.transform.position) <= 1.1f){
			//Then attack	
			Debug.Log("I ATTACK!");
			hasAttackAction = false;
		}
		else if(hasMovePoints) {
			//Else approach target
			aController.ApproachTarget(target.transform);
			Debug.Log("I approach");
		}
		else {
			Debug.Log("Passing turn");
			myturn = false;
			combat.NewTurn();
		}
	}

	void SelectTarget(){
		//find closest target
		float shortest = Mathf.Infinity;
		int closest = -1;
		for (int i = 0; i < enemies.Length; i++) {
			float dist = Vector3.Distance(transform.position, enemies[i].transform.position);
			if (dist < shortest){
				shortest = dist;
				closest = i;
			}
		}
		if (closest != -1){
			target = enemies[closest];
			Debug.Log("Selected "+target.charName+" as target.");
		}
	}

	public void GroupAwareness(CharacterSheet[] group_enemies){
		enemies = group_enemies;
	}
}