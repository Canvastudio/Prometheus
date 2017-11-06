using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtFxMissileBezier :  ArtFxBase {

	public ArtBezierMover mover;

	public override void Init(Vector3 _starPos, Vector3 _endPos)
	{
		base.Init(_starPos, _endPos);
		mover.SetP(ArtMath.Bezier3Pos(startPos, endPos));
	}

	public void OnDrawGizmos() {

		mover = this.GetComponent<ArtBezierMover>();

	}

}
