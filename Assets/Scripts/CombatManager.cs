using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CombatManager : MonoBehaviour {

	CharacterSheet[] participants;
	BasicEnemy[] ais;
	//string[] participants;
	int[] initiative;
	int cur_round; //switches round for ticking abilities and stuff
	int cur_turn; //lists through all participants

	GUIManager ui;

	// Use this for initialization
	void Start () {
		participants = new CharacterSheet[0];
		initiative = new int[0];

		ui = GetComponent<GUIManager>();
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
			BasicEnemy tempAi = ais[i];
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
			ais[i] = ais[index];
			initiative[i] = initiative[index];
			//Assign saved values to found index
			//Debug.Log("Moving "+tempSheet + " to "+index);
			participants[index] = tempSheet;
			ais[index] = tempAi;
			initiative[index] = tempInit;
		}

	}

	public void NewTurn(){
		cur_turn ++;
		if (cur_turn > participants.Length){
			NewRound();
		}
		else{
			GiveControl();
		}
	}
	public void NewRound(){
		cur_turn = 0;
		cur_round ++;
		GiveControl();
	}

	void GiveControl(){
		if (participants[cur_turn].aiControlled){
			Debug.Log("It's "+participants[cur_turn].charName + " turn! " );
			ais[cur_turn].StartCombatTurn();
			ui.skip.interactable = false;
		}
		else{
			Debug.Log("It's player turn");
			ui.skip.interactable = true;
		}
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

	public void SetParticipants(CharacterSheet[] sheets){
		participants = sheets;
		initiative = new int[participants.Length];
		ais = new BasicEnemy[participants.Length];
		//Check AI and roll initiative
		for (int i = 0; i < participants.Length; i++) {
			if (participants[i].aiControlled){
				ais[i] = participants[i].gameObject.GetComponent<BasicEnemy>();
			}
			initiative[i] = BaseRules.Roll("1d20+"+participants[i].GetStat("Dexterity"));
		}
		Sort();

		cur_turn = 0;
		cur_round = 0;
		GiveControl();
	}


	void OnGUI(){
		for (int i = 0; i < participants.Length; i++) {
			GUI.Label(new Rect(10, 10+20*i, 200, 40), participants[i].charName +" "+ initiative[i]);
		}
	}
		
}
