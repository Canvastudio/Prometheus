using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtFxMissileBezier :  ArtFxBase {

	public ArtBezierMover mover;

	public List<Vector3> berizerPos = null;

	public override void Init(Vector3 _starPos, Vector3 _endPos, Callback _OnHit)
	{
		base.Init(_starPos, _endPos, _OnHit);
		berizerPos = ArtMath.Bezier3Pos(startPos, endPos);
		mover.SetPos(berizerPos);
	}

	public override void Init(Transform tran_start, Transform tran_end, Callback _OnHit)
	{
		base.Init(tran_start, tran_end, _OnHit);
		berizerPos = ArtMath.Bezier3Pos(startPos, endPos);
		mover.SetPos(berizerPos);
	}

	public override void UpdatePos()
	{
		tran_end.SetPos(ref endPos);
		berizerPos[3] = endPos;
		mover.UpdatePos(berizerPos);

		base.UpdatePos();
	}

	public void OnDrawGizmos() {

		mover = this.GetComponent<ArtBezierMover>();
		SetLayer("StageView");

	}

}
