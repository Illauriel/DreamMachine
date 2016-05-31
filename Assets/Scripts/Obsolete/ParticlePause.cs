using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticlePause : MonoBehaviour {

	ParticleSystem[] particles;
	// Use this for initialization
	void Start () {
		ParticleSystem root_particle = GetComponent<ParticleSystem>();
		//Uncomment in case "Pause (withChildren)" doesnt work
		/*List<ParticleSystem> found_particles = new List<ParticleSystem>();

		ParticleSystem[] child_particles = GetComponentsInChildren<ParticleSystem>();
		if (root_particle != null){
			found_particles.Add(root_particle);
		}
		if (child_particles.Length > 0){
			for (int i = 0; i < child_particles.Length; i++) {
				found_particles.Add(child_particles[i]);
			}
		}
		particles = found_particles.ToArray();*/
		if (root_particle != null){
			particles = new ParticleSystem[1];
			particles[0] = root_particle;
		}
		else{
			Debug.LogError("Root object doesn't contain a particle system");
		}
	}
	
	public void Pause(){
		for (int i = 0; i < particles.Length; i++) {
			particles[i].Pause(true);
		}
	}
	public void Unpause(){
		for (int i = 0; i < particles.Length; i++) {
			particles[i].Play(true);
		}
	}


}
