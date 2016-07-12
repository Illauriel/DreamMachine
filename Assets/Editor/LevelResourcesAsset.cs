using UnityEngine;
using UnityEditor;

public class LevelResourcesAsset
{
	[MenuItem("Assets/Create/LevelResources")]
	public static void CreateAsset ()
	{
		ScriptableObjectUtility.CreateAsset<LevelResources> ();
	}
}