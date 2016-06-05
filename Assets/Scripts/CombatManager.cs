using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CombatManager : MonoBehaviour {

	CharacterSheet[] participants;
	//string[] participants;
	int[] initiative;
	int cur_round; //switches round for ticking abilities and stuff
	int cur_turn; //lists through all participants


	// Use this for initialization
	void Start () {
		/* //Test with strings instead of charsheets
		participants = new string[]{"Mikasa", "Yoba", "Taylan", "Miko", "Hitler"};
		initiative = new int[]{21, 15, 1, 22, 40};
		Sort();
		for (int i = 0; i < participants.Length; i++) {
			Debug.Log(i+". "+participants[i] +" with init "+initiative[i]);
		}*/
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Sort(){
		for (int i = 0; i < participants.Length; i++) {
			//save current values for this index
			CharacterSheet tempSheet = participants[i];
			//string tempSheet = participants[i];
			int tempInit = initiative[i];
			//get index of highest value

			int highestInit = -1;
			int index = -1;
			for (int j = i; j < participants.Length; j++) {
				if (initiative[j] > highestInit){
					highestInit = initiative[j];
					index = j;
				}	
			}		
			//Debug.Log(i+"Winner is "+participants[index]);
			//Assign highest found value to this index
			//Debug.Log(i+);

			participants[i] = participants[index];
			initiative[i] = initiative[index];
			//Assign saved values to found index
			//Debug.Log("Moving "+tempSheet + " to "+index);
			participants[index] = tempSheet;
			initiative[index] = tempInit;
		}

	}

	public void NewTurn(){
		cur_turn ++;
		if (cur_turn > participants.Length){
			cur_turn = 0;
			NewRound();
		}
	}
	public void NewRound(){
		cur_round ++;
	}

	public void ResolveAttack(CharacterSheet attacker, CharacterSheet victim){
		int attackRoll = BaseRules.Roll("1d20"+attacker.GetStat("Strength"));
		if (attackRoll >= victim.armorClass){
			Debug.Log("Attack succesful!");
			//Determine damage
			int damage = BaseRules.Roll(attacker.cur_weapon.damage);
			victim.cur_hp -= damage;
			Debug.Log(victim.charName+ " took "+damage+" damage." );
		}
	}

}
