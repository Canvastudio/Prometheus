using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtTestRay : MonoBehaviour {

	public Transform tranStart;
	public Transform tranEnd;

	public ArtFxRay artRay;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

		artRay.Init(tranStart.position, tranEnd.position, null);

	}
}
