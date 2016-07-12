using UnityEngine;
using UnityEditor;

public class VowelSheetAsset
{
	[MenuItem("Assets/Create/VowelSheet")]
	public static void CreateAsset ()
	{
		ScriptableObjectUtility.CreateAsset<VowelSheet> ();
	}
}