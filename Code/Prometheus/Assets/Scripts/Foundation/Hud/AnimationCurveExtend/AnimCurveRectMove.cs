using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimCurveRectMove : AnimCurveBase {

	public RectTransform tran;

	public Vector2 startpos;
	public Vector2 endpos;
	public Vector2 movepos;
	public Vector2 cur_pos;

	void Awake() {
	
		movepos = endpos - startpos;

		tran = this.GetComponent<RectTransform>();
	
	}

	public void OnEnable() {

		if(isenable) {

			tran.anchoredPosition = startpos;
			OnStart();

		}

	}

	public override void SetValue ()
	{
		base.SetValue ();

		cur_pos = movepos * cur_value + startpos;

		tran.anchoredPosition = cur_pos;

	}
}
