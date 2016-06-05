using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class CharacterSheet : MonoBehaviour {

	public TextAsset charSheet;
	public TextAsset weaponsList;

	public string charName;
	public bool aiControlled;
	private int level;//should set it separately
	//Primary Stats!
	int[] statValues;
	int[] statBonuses;
	string[] statNames = new string[]{"Strength", "Dexterity", "Constitution", "Intelligence", "Wisdom", "Charisma"};


	//Deriviates
	public int speed;
	public int hp_base;
	public int cur_hp;

	public int armorClass;

	public Sprite portrait;

	public Weapon cur_weapon;
	// Use this for initialization
	void Start () {
		Initialize();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public int GetStat (string name){
		int index = -1;
		for (int i = 0; i < statNames.Length; i++) {
			if (statNames[i] == name){
				index = i;
			}
		}
		if (index == -1){
			Debug.LogError("Stat "+name+" is not registered!");
		}
		return statValues[index] + statBonuses[index];
	}

	void Initialize(){
		Debug.Log(name + " has "+charSheet);
		statValues = RulesReader.GetStats(new string[]{"str","dex","con","int","wis","cha"}, charSheet.text);

		cur_weapon = RulesReader.GetWeapon("sword1", weaponsList.text);
	}

}
