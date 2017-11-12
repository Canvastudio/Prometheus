using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtFxMissile : ArtFxBase {

	public ArtLineMover mover;
	public List<Vector3> plist = new List<Vector3>() {
	
		Vector3.zero,
		Vector3.zero
	
	};

	public override void Init(Vector3 _starPos, Vector3 _endPos, Callback _OnHit = null)
	{
		base.Init(_starPos, _endPos, _OnHit);
		plist[0] = tran_start.position;
		plist[1] = tran_end.position;
		mover.SetPos(plist);
	}

	public override void Init(Transform tran_start, Transform tran_end, Callback _OnHit = null)
	{
		base.Init(tran_start, tran_end, _OnHit);
		plist[0] = tran_start.position;
		plist[1] = tran_end.position;
		mover.SetPos(plist);

	}

	public override void UpdatePos()
	{
		tran_end.SetPos(ref endPos);
		plist[1] = endPos;
		mover.UpdatePos(plist);
		base.UpdatePos();
	}

	public void OnDrawGizmos() {

		mover = this.GetComponent<ArtLineMover>();

	}

}
