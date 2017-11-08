using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtMover : MonoBehaviour {

	public Vector3 endPos;

	public bool isStart = false;

	public Transform tran;

	public ArtFxBase fxbase = null;

	protected void OnEnd() {

		isStart = false;
		FxPool.Recover(this.gameObject);

		if (fxbase != null)
			fxbase.OnEnd();

	}

	public virtual void SetPos(List<Vector3> _plist){
	}

	public void OnDrawGizmos() {

		tran = this.transform;
		fxbase = this.GetComponent<ArtFxBase>();

	}

}
