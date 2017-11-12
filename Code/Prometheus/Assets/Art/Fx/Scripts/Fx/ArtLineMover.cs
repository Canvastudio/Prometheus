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
	public float temp_len;
	public float cur_len;

	
	// Update is called once per frame
	void Update () {

		if (!isStart)
			return;

		tran.position = Vector3.MoveTowards(tran.position, next_pos, Time.deltaTime * velocity);
		dir = (next_pos - tran.position).normalized;
		tran.rotation = Quaternion.Lerp(tran.rotation, ArtMath.LookAtZ(dir), Time.deltaTime * 5);

		DetectNext();
		
	}

	public override void SetPos(List<Vector3> _plist) {

		plist = _plist;
		k = 0;
		n = plist.Count - 1;
		tran.position = plist[k];

		OnNext();

		isStart = true;

	}

	public void UpdatePos(List<Vector3> _plist) {

		plist = _plist;

		if ((k + 1) == n)
			next_pos = plist[n];

	}

	public void DetectNext() {
	
		if (Vector3.Distance(tran.position, next_pos) < .1f) {

			k++;

			if (k + 1 > n) {
				OnEnd();
				return;
			}

			OnNext();

		
		}
	
	}

	private void OnNext() {
		next_pos = plist[k + 1];
	}

}
