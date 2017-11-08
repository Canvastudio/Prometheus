using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtLineMover : ArtMover {

	public List<Vector3> plist = new List<Vector3>(2);
	public float m_time;
	public float total_time = 3;
	public float velocity = 1.3f;

	public int k;
	public int n = 4;

	private Vector3 cur_pos = Vector3.zero;
	private Vector3 start_pos;
	private Vector3 next_pos;
	private Vector3 dir;

	public float move_len;
	public float cur_len;

	
	// Update is called once per frame
	void Update () {

		if (!isStart)
			return;
		
		m_time += Time.deltaTime;
		cur_len = velocity * m_time;

		cur_pos = start_pos + dir * cur_len;

		tran.position = cur_pos;

		DetectNext();
		
	}

	public override void SetPos(List<Vector3> _plist) {

		plist.Clear();
		plist = _plist;

		m_time = 0;
		k = 0;
		n = plist.Count - 1;
		cur_len = 0;
		move_len = 0;

		tran.position = plist[k];
		cur_pos = _plist[k];

		OnNext();

		isStart = true;

	}

	public void DetectNext() {
	
		if (cur_len > move_len) {
		
			k++;

			if (k + 1 > n) {
				OnEnd();
				return;
			}

			OnNext();

		
		}
	
	}

	private void OnNext() {
	
		start_pos = plist[k];
		next_pos = plist[k + 1];
		dir = (next_pos - start_pos).normalized;
		move_len += Vector3.Distance(start_pos, next_pos);
		tran.rotation = ArtMath.LookAtZ(dir);
	
	}

}
