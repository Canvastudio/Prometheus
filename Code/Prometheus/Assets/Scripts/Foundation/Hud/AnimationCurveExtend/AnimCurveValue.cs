using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimCurveValue : AnimCurveBase {

	public float m_value = 0;

	public override void SetValue ()
	{
		base.SetValue ();

		m_value = cur_value;

	}

}
