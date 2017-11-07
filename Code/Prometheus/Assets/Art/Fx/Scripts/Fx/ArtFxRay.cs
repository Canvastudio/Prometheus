using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtFxRay : ArtFxBase {

	public Transform tranStart;
	public Transform tranEnd;

	public ArtTextureRoll[] rolllist;

	public float offsetdir = 0.5f;


	public override void Init(Vector3 _startPos, Vector3 _endPos, Callback _OnHit) {

		base.Init(_startPos, _endPos, _OnHit);

		Vector3 dir = (endPos - startPos).normalized;

		float len = Vector3.Distance(startPos, endPos) - offsetdir;

		Vector3 angle = new Vector3(0, 0, ArtMath.angle_360(startPos, endPos));

		for (int i = 0; i < rolllist.Length; i++) {

			rolllist[i].tran.position = startPos + dir * offsetdir;
			rolllist[i].tran.eulerAngles = angle; 
			rolllist[i].SetLen(len);

		}

		tranStart.eulerAngles = angle;
		tranEnd.eulerAngles = angle;

		tranStart.position = startPos;
		tranEnd.position = endPos;
	}

	public void OnDrawGizmos() {

		rolllist = this.GetComponentsInChildren<ArtTextureRoll>();
		tranStart = this.transform.Find("ps_start");
		tranEnd = this.transform.Find("ps_end");

	}
}
