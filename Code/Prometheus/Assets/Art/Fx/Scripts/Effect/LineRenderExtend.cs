using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRenderExtend : MonoBehaviour {

	public AtkRangeEffect rangeEffect;

	public LineRenderer linerender;

	public AnimationCurve anim_alpha;

	public Color color_alpha = Color.white;

	public float m_time;

	public float width_rate = 3.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		UpdateTime();
		UpdateAlpha();
		UpdateWidth();
		
	}

	public void UpdateTime() {
		
		m_time = rangeEffect.br/1.2f;

	}

	public void UpdateAlpha() {
		
		color_alpha.a = anim_alpha.Evaluate(m_time);
		linerender.startColor = color_alpha;
		linerender.endColor = color_alpha;

	}

	public void UpdateWidth() {
		
		linerender.widthMultiplier = anim_alpha.Evaluate(m_time) / width_rate;

	}

}
