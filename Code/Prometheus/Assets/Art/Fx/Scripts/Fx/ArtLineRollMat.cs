using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtLineRollMat : MonoBehaviour {

	public Material mat;
	public Vector2 rollVec2;
	public float roll_speed = 2;

	// Use this for initialization
	void Start () {
	
		mat = this.GetComponent<LineRenderer>().sharedMaterial;

	}
	
	// Update is called once per frame
	void Update () {

		rollVec2.x -= Time.deltaTime * roll_speed;
		mat.mainTextureOffset = rollVec2;

	}
}
