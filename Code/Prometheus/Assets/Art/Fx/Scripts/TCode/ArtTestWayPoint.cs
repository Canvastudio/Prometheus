using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtTestWayPoint : MonoBehaviour {

	public Vector3[] test_point;

	public ArtWayPoint artWay;

	// Use this for initialization
	void Start () {

		artWay.SetWayPoints(test_point);

	}

	// Update is called once per frame
	void Update () {

	}
}
