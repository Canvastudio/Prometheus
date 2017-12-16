using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtFxDrop : ArtFxBase {

	public ParticleSystem particle;

	public float back_time = 1.7f;

	public float cool_back_time = 0;

	public ParticleSystem.Particle[] particlChilds;

	public bool ishit = false;

	public ParticleSystem particleHit;

	public int parical_count = 0;
	
	public Vector3 dropPos;

	public float speed = 0.1f;

	public int maxpar = 40;

	public List<float> randomlist;

	void Awake() {
	
		particlChilds = new ParticleSystem.Particle[maxpar];

		randomlist = new List<float>(maxpar);

		for(int i = 0; i < maxpar; i++)
			randomlist.Add(speed + Random.Range(-3.5f, 3f));

	}

	public override void Show(Vector3 pos, int count){

		base.Show(pos, count);

		ParticleSystem.MainModule main = particle.main;
		
		Debug.Log("ParticleSystem count:" + count);

		parical_count = count;

		main.maxParticles = Mathf.Min(count, maxpar);

		particle.Play();
		particle.Emit(count);

		for(int i = 0; i < maxpar; i++)
			randomlist[i] = speed + Random.Range(-.5f, .5f);	

		cool_back_time = 0;

		particleHit.transform.position = dropPos;

	}

	public override void UpLogic() {

		cool_back_time += Time.deltaTime;

		if(cool_back_time <= back_time) {
			return;
		}

		particle.GetParticles(particlChilds);

		//ishit = false;

		for(int i = 0; i < parical_count; i++) {

			particlChilds[i].position = Vector3.Lerp(particlChilds[i].position, dropPos, Time.deltaTime * randomlist[i]);

			//particlChilds[i].size = Vector3.Lerp(particlChilds[i].size, finalSize, Time.deltaTime * randomlist[i]);

			if(Vector3.Distance(particlChilds[i].position, dropPos) < 0.1f) {
				particlChilds[i].remainingLifetime = 0;
				ishit = true;
				particleHit.Emit(1);
			}	
			
		}

		particle.SetParticles(particlChilds, parical_count);

		//if(ishit)
			//particleHit.Emit(2);

	}

}
