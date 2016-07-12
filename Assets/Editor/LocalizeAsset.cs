using UnityEngine;
using UnityEditor;

public class LocalizeAsset
{
	[MenuItem("Assets/Create/Localize")]
	public static void CreateAsset ()
	{
		ScriptableObjectUtility.CreateAsset<Localize> ();
	}
}