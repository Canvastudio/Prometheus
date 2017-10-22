using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimCurveMove : AnimCurveBase {

	public Transform tran;

	public Vector3 pos;

	public Vector3 cur_pos;

	public void OnEnable() {

		if(isenable) {
		
			this.transform.localPosition = pos;
			OnStart();
		
		}

	}

	public override void SetValue ()
	{
		base.SetValue ();

		cur_pos = pos * cur_value;

		tran.localPosition = cur_pos;

	}

	public void OnDrawGizmos() {

		tran = this.transform;
		pos = tran.localPosition;

	}
}
