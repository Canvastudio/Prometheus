using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtFxProperty : MonoBehaviour {

	public ParticleSystem[] particls;

	public void SetSize(float size) {
	
		for (int i = 0; i < particls.Length; i++) {
		
			ParticleSystem.MainModule main = particls[i].main;
			main.startSizeMultiplier = size;
		
		}
	
	}

	public void OnDrawGizmos() {

		SetLayer("StageView");

		particls = this.GetComponentsInChildren<ParticleSystem>(true);

	}

	public string layername = "StageUI";

	public void SetLayer(string _layername) {

		layername = _layername;

		ParticleSystemRenderer[] prenders = this.GetComponentsInChildren<ParticleSystemRenderer>(true);

		for(int i = 0; i < prenders.Length; i++) {

			prenders[i].sortingLayerName = layername;

		}

	}

}
