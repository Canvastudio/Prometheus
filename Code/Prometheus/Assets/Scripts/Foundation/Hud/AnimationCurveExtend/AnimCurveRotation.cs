using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimCurveRotation : AnimCurveBase {

	public Transform tran;
	private Vector3 m_angle;

	public float scale_angle = 360;

	public void Start() {
	
		OnStart();
	
	}

	public override void SetValue ()
	{
		base.SetValue ();

		m_angle.z = cur_value * scale_angle;

		tran.eulerAngles = m_angle;

	}

	public override void OnEnd ()
	{
		base.OnEnd ();

		OnStart();

	}

	public void OnDrawGizmos() {

		tran = this.transform;
		m_angle = tran.eulerAngles;

	}

}
