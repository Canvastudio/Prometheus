using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimCurveScale : AnimCurveBase {

	public Transform tran;

	private Vector3 m_scale;

	public override void SetValue ()
	{
		base.SetValue ();

		m_scale = Vector3.one * cur_value;

		tran.localScale = m_scale;

	}

	public void OnDrawGizmos() {

		tran = this.transform;
		m_scale = tran.localScale;

	}

}
