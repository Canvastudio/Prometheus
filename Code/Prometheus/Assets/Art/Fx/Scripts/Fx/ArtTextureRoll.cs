using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtTextureRoll : MonoBehaviour {

	public ParticleSystemRenderer psRender;
	public Material mat;
	public float Length;

	public float roll_scale_rate = 1.97f;
	public float roll_speed = 2;
	public Vector2 rollVec2;

	public Transform tran;

	// Use this for initialization
	void Start () {


	}
	
	// Update is called once per frame
	void Update () {

		rollVec2.x -= Time.deltaTime * roll_speed;
		mat.mainTextureOffset = rollVec2;
		
	}

	public void SetLen(float _len) {
	
		Length = _len * roll_scale_rate;
		psRender.lengthScale = Length;
	
	}

	public void OnDrawGizmos() {

		psRender = this.GetComponent<ParticleSystemRenderer>();
		mat = psRender.sharedMaterial;
		tran = this.transform;
	
	}

}
