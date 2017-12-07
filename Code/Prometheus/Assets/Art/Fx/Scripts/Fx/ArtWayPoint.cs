using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtWayPoint : MonoBehaviour {

	public LineRenderer lineRender;

	public ParticleSystem endParticle;

	//public Vector3[] test_point;

	//// Use this for initialization
	//void Start () {

	//	SetWayPoints(test_point);
		
	//}
	
	//// Update is called once per frame
	//void Update () {
		
	//}

	public void SetWayPoints(Vector3[] ways) {
	
		lineRender.positionCount = ways.Length;

		lineRender.SetPositions(ways);

		endParticle.transform.position = ways[ways.Length - 1];
	
	}
}
