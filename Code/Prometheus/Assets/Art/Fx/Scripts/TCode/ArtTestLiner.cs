using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtTestLiner : MonoBehaviour {

	public List<Transform> tranlist;

	public ArtFxBase mover;

	// Use this for initialization
	void Start () {

		//mover.SetP(tranlist[0].position, tranlist[1].position, tranlist[2].position, tranlist[3].position);
		InvokeRepeating("CreateOne", 0.3f, 2f);

	}

	public void CreateOne() {

		ArtFxBase artBezier = GameObject.Instantiate(mover.gameObject).GetComponent<ArtFxBase>();

		artBezier.Init(tranlist[0].position, tranlist[1].position, null);


	}
}
