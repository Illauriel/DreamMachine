using UnityEngine;
using System.Collections;

public class Weapon {

	public enum DamageType{Piercing, Slashing, Crushing, Fire, Cold, Lightning};
	public DamageType damageType;
	public string w_name;
	public string description;
	public string damage;
	public int reach;

}
