using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtBezierMover : ArtMover {

	public List<Vector3> plist = new List<Vector3>(4);
	public float m_time;
	public float total_time = 3;
	public float bezierA;

	public int k;
	public int n = 4;

	private Vector3 cur_pos = Vector3.zero;
	private Vector3 before_pos;
	private Vector3 dir;

	
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
		tran.position = Vector3.Lerp(tran.position, cur_pos, Time.deltaTime * 2);

		dir = tran.position - before_pos;
		tran.rotation = ArtMath.LookAtZ(dir);

		m_time += Time.deltaTime / total_time;
		m_time = Mathf.Min(m_time, 1);

		bezierA = 1 - m_time;

		DetectEnd();

	}

	public override void SetPos(List<Vector3> _plist) {
	
		plist.Clear();
		plist.Add(_plist[0]);
		plist.Add(_plist[1]);
		plist.Add(_plist[2]);
		plist.Add(_plist[3]);

		m_time = 0;
		bezierA = 1;

		tran.position = plist[0];
		before_pos = _plist[0];
		cur_pos = _plist[0];

		isStart = true;

	}

	public void DetectEnd() {

		if (Vector3.Distance(tran.position, plist[3]) < 0.1f) {

			OnEnd();

		}
	
	}



}
