using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtFxDrop : ArtFxBase {

	public ParticleSystem particle;

	public float back_time = 1.7f;

	public float cool_back_time = 0;

	public ParticleSystem.Particle[] particlChilds;

	public int parical_count = 0;

	public Vector3 dropPos;

	public float speed = 0.1f;

	public int maxpar = 40;

	void Awake() {
	
		particlChilds = new ParticleSystem.Particle[maxpar];

	}

	public override void Show(Vector3 pos, int count){

		base.Show(pos, count);

		//ParticleSystem.MainModule main = particle.main;
		
		//main.maxParticles = Mathf.Min(count, maxpar);

		particle.Emit(count);
		//particle.Simulate(3f);

		parical_count = particle.GetParticles(particlChilds); 

		cool_back_time = 0;

	}

	public override void UpLogic() {

		cool_back_time += Time.deltaTime;

		if(cool_back_time <= back_time) {
			return;
		}

		Debug.Log(particlChilds.Length);

		for(int i = 0; i < parical_count; i++) {

			particlChilds[i].position = Vector3.Lerp(particlChilds[i].position, dropPos, Time.deltaTime * speed);

			Debug.Log(particlChilds[i].position);
			
		}
	}

}
