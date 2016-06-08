using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelInitiator : MonoBehaviour {




	/*public static NavMeshAgent GetAgent (string tag){
		GameObject[] objs = GameObject.FindGameObjectsWithTag(tag);
		if (objs.Length > 1){
			Debug.LogWarning("More than one object with tag \"" + tag + "\" found!");
		}
		NavMeshAgent result = objs[0].GetComponent<NavMeshAgent>();
		if (result == null){
			Debug.LogError("The object doesn't have a NavMeshAgent!");
		}
		return result;
			
	}*/

	/*public static NavMeshAgent GetAgent (GameObject obj){
		NavMeshAgent result = obj.GetComponent<NavMeshAgent>();
		if (result == null){
			Debug.LogError("The object doesn't have a NavMeshAgent!");
		}
		return result;
	}*/

	/*public static AgentController GetAgentController(GameObject obj){
		AgentController result = obj.GetComponent<AgentController>();
		if (result == null){
			Debug.LogError("The object doesn't have an AgentController!");
		}
		return result;
	}

	public static AgentController[] GetAgentControllers(){
		AgentController[] result = GameObject.FindObjectsOfType<AgentController>();
		if (result.Length == 0){
			Debug.LogWarning("No Agents found in scene!");
		}
		return result;
	}*/
	public static T[] GetComponentsOnObjects<T>(GameObject[] objs) where T:UnityEngine.Component{
		T[] result = new T[objs.Length];
		for (int i = 0; i < objs.Length; i++) {
			T component = objs[i].GetComponent<T>();
			if (component == null){
				Debug.LogError("Object "+objs[i].name+" dowsn't have a requested Component!");
			}
			result[i] = component;
		}

		return result;
	}
	
	/*public static GameObject FindGameobjectOfComponent(Component component){
		GameObject result = component.gameObject;
		return result;
	}*/
	public static GameObject[] FindGameObjectsOfComponents(Component[] components){
		GameObject[] result = new GameObject[components.Length];
		for (int i = 0; i < components.Length; i++) {
			result[i] = components[i].gameObject;

		}
		return result;
	}

	public static T[] FindObjectsOfTypeWithTag<T> (string tag) where T:UnityEngine.Component{
		List<T> found_objects = new List<T>();
		T[] all_objects = GameObject.FindObjectsOfType<T>();
		for (int i = 0; i < all_objects.Length; i++) {
			if (all_objects[i].tag == tag){
				found_objects.Add(all_objects[i]);
			}
		}

		if (found_objects.Count == 0){
			Debug.LogWarning("No objects of requested type with tag "+tag+" found!");
		}
		return found_objects.ToArray();
	}

	/*public static GameController GetGameController(){
		GameController result = null;
		GameObject obj = GameObject.Find("GameController");
		if (obj != null){
			result = obj.GetComponent<GameController>();
		}
		else {
			Debug.LogError("There is no GameController object in the scene!");
		}
		return result;
	}*/

	/*public static ParticleSystem[] GetParticlesWithTag(string tag){
	List<ParticleSystem> found_particles = new List<AgentController>();
	AgentController[] all_controllers = GameObject.FindObjectsOfType<AgentController>();
	for (int i = 0; i < all_controllers.Length; i++) {
		if (all_controllers[i].tag == tag){
			found_controllers.Add(all_controllers[i]);
		}
	}
	if (found_controllers.Count == 0){
		Debug.LogWarning("No Agents with tag "+tag+" found!");
	}
	return found_controllers.ToArray();
	}*/
	/*public static AgentController[] GetAgentControllers(string tag){
	List<AgentController> found_controllers = new List<AgentController>();
	AgentController[] all_controllers = GameObject.FindObjectsOfType<AgentController>();
	for (int i = 0; i < all_controllers.Length; i++) {
		if (all_controllers[i].tag == tag){
			found_controllers.Add(all_controllers[i]);
		}
	}
	if (found_controllers.Count == 0){
		Debug.LogWarning("No Agents with tag "+tag+" found!");
	}
	return found_controllers.ToArray();
	}*/
}
