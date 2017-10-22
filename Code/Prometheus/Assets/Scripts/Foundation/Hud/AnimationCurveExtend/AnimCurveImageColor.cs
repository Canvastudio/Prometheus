using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimCurveImageColor : AnimCurveBase {

	public Image m_image;

	public Color m_color;

	public override void SetValue ()
	{
		base.SetValue ();

		m_color.a = cur_value;

		m_image.color = m_color;

	}

	public void OnDrawGizmos() {
	
		m_image = this.GetComponent<Image>();
		m_color = m_image.color;
	
	}

}
