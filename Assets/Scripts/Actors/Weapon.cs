using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

	public enum DamageType{Piercing, Slashing, Crushing, Fire, Cold, Lightning};
	public DamageType damageType;
	public string name;
	public string description;
	public string damage;
	public int reach;

}
