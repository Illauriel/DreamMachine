using UnityEngine;
using System.Collections;

public class BaseRules {

	// Use this for initialization
	void Start () {
	
	}



	public static int Roll(int n, int d, int bonus){
		int result = 0;
		for (int i = 0; i < n; i++) {
			int roll = Random.Range(1, d+1);
			//Debug.Log("1d"+d+"="+roll);
			result += roll;
		}

		return result + bonus;
	}

	public static int Roll(string my_roll){
		//takes "#d@+n" string and converts it to dice rolls
		int number = 1;
		int dice = 6;
		int bonus = 0;

		string[] substrings = new string[0];
		//Remove all spaces
		my_roll = my_roll.Replace(" ", "");

		//Figure out bonus or pealty
		if (my_roll.Contains("+")){
			substrings = my_roll.Split('+');
			bonus = int.Parse(substrings[1]);
		}
		else if (my_roll.Contains("-")){
			substrings = my_roll.Split('-');
			bonus = -int.Parse(substrings[1]);
		}
		else {
			substrings = new string[]{ my_roll };
		}
		//Debug.Log("Bonus is "+bonus);
		//Now get dice and their number
		substrings = substrings[0].Split('d');
		number = int.Parse(substrings[0]);
		if (substrings[1] != ""){
			dice = int.Parse(substrings[1]);
		}

		return Roll(number, dice, bonus);
	}

}
