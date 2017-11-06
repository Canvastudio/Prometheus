using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtTestBezier : MonoBehaviour {

	public List<Transform> tranlist;

	public ArtBezierMover mover;

	// Use this for initialization
	void Start () {

		//mover.SetP(tranlist[0].position, tranlist[1].position, tranlist[2].position, tranlist[3].position);

		InvokeRepeating("CreateBezier", 0.3f, 0.5f);
		
	}

	public void CreateBezier() {

		ArtFxMissileBezier artBezier = GameObject.Instantiate(mover.gameObject).GetComponent<ArtFxMissileBezier>();

		artBezier.Init(tranlist[0].position, tranlist[3].position);

	
	}

}
