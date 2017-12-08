using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtEnableChangeLifeTime : MonoBehaviour {

	public ParticleSystem particle;

	public ParticleSystem.MainModule main;

	public float min_time = 1.1f;
	public float max_time = 2.1f;

	public virtual void OnEnable() {

		if (particle != null) {
			main = particle.main;
			main.startLifetimeMultiplier = Random.Range(min_time, max_time);
		}

	}

	public void OnDrawGizmos() {
		particle = this.GetComponent<ParticleSystem>();
	}

}
