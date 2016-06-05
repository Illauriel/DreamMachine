using UnityEngine;
using System.Collections;

public class BasicEnemy : MonoBehaviour {

	//state machine 
	public enum State {Patrol, Interested, Hunt, Combat};
	public State state; 

	AIPerception percept;
	AgentController aController;
	CharacterSheet charsheet;

	GameController gc;

	// Use this for initialization
	void Start () {
		percept = GetComponent<AIPerception>();
		aController = GetComponent<AgentController>();
		charsheet = GetComponent<CharacterSheet>();
		//And start following patrol nodes;
		PatrolStateTransition();

		gc = GameObject.FindObjectOfType<GameController>();
		charsheet.aiControlled = true;
	}
	
	// Update is called once per frame
	void Update () {
		StateSolver();
	}

	void AIPatrol(){
		aController.MoveInPath(aController.walkingSpeed);

	}

	void StateSolver(){
		if (state == State.Patrol){
			AIPatrol();
		}

		if (percept.GetTarget() != null && state != State.Combat){
			CombatStateTransition();
		}
	}

	void PatrolStateTransition(){
		state = State.Patrol;
		aController.StartPatrol();
	}

	void CombatStateTransition(){
		state = State.Combat;
		gc.StartCombat();
	}

	public void MakeCombatTurn(){
		
	}
}
