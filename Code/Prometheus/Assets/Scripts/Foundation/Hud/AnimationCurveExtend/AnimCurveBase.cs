using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimCurveBase : MonoBehaviour {

	public float total_time = 0.4f;
	private float m_time = 0;
	private float temp_time = 0;
	protected float cur_value = 0;

	public AnimationCurve animCurve;

	public bool isready = true;

	public Callback EndHandler = null;

	public bool isenable = false;

	public virtual void OnStart() {

		isready = false;

	}

	public virtual void OnEnd() {

		isready = true;

		m_time = 0;
		temp_time = 0;

		if(EndHandler != null) EndHandler();
	
	}

	protected void Update() {
	
		if(isready) return;

		m_time += Time.unscaledDeltaTime;

		temp_time = m_time / total_time;

		cur_value = animCurve.Evaluate(temp_time);

		SetValue();

		if(m_time >= total_time) {

			cur_value = animCurve.Evaluate(1);
			SetValue();

			OnEnd();

			return;

		}
	
	}

	public virtual void SetValue() {}

	public virtual void ReSet() {

		m_time = 0;
		m_time = 0;
		temp_time = 0;
	
	}
}
