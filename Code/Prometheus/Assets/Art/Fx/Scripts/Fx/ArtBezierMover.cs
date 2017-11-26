using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtBezierMover : ArtMover {

	public List<Vector3> plist = null; 
	public float m_time;
	public float total_time = 3;
	public float bezierA;

	public int k;
	public int n = 4;

	private Vector3 cur_pos = Vector3.zero;
	private Vector3 before_pos;
	private Vector3 dir;

	//public List<Vector3> buffer = new List<Vector3>(100);
	
	// Update is called once per frame
	void Update () {

		if (!isStart)
			return;

		before_pos = cur_pos;
		cur_pos = Vector3.zero;

		for (k = 0; k < n - 1; k++)
			cur_pos += ArtMath.Bezier3RatioList[k] * ArtMath.PowerOfN(m_time, k) * ArtMath.PowerOfN(bezierA, n - k) * plist[k];

		cur_pos += ArtMath.PowerOfN(m_time, k) * plist[k];
		cur_pos.z = 0;

		before_pos = tran.position;
		tran.position = Vector3.Lerp(tran.position, cur_pos, Time.deltaTime * 10);

		dir = tran.position - before_pos;
		tran.rotation = ArtMath.LookAtZ(dir);

		m_time += Time.deltaTime / total_time;
		m_time = Mathf.Min(m_time, 1);

		bezierA = 1 - m_time;

		DetectEnd();

	}

	public override void SetPos(List<Vector3> _plist) {
	
		plist = _plist;

		m_time = 0;
		bezierA = 1;

		tran.position = plist[0];
		before_pos = plist[0];
		cur_pos = plist[0];

		isStart = true;

	}

	public void UpdatePos(List<Vector3> _plist) {
	
		//plist = _plist;

		//endPos = plist[3];
		//endPos.z = tran.position.z;
	
	}

	public void DetectEnd() {

		if (m_time == 1) {

			OnEnd();

		}
	
	}



}
