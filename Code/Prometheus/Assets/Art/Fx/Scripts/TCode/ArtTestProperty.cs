using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtTestProperty : MonoBehaviour {

	public ArtFxProperty artFx;

	// Use this for initialization
	void Start () {

		artFx.SetSize(Random.Range(3, 10));
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
