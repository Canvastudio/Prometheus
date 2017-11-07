using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtTestCG : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
		//List<Vector3> list = ArtVector3Pool.Get();

		List<Vector3> list = new List<Vector3>(1000);

		//Debug.Log(list.Count);

		for (int i = 0; i < 10; i++) {

			list.Add(new Vector3());

		}

		//list.Clear();

	}
}
