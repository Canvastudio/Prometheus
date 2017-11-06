using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtFxBase : MonoBehaviour {

	public Vector3 startPos;
	public Vector3 endPos;

	public Transform tran;

	public virtual void Init(Vector3 _starPos, Vector3 _endPos) {
	
		startPos = _starPos;
		endPos = _endPos;

		tran = this.transform;
		tran.position = startPos;
	
	}

}
