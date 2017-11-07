using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtFxMissile : ArtFxBase {

	public ArtLineMover mover;

	public override void Init(Vector3 _starPos, Vector3 _endPos, Callback _OnHit = null)
	{
		base.Init(_starPos, _endPos, _OnHit);
		List<Vector3> plist = new List<Vector3>(2);
		plist.Add(_starPos);
		plist.Add(_endPos);
		mover.SetPos(plist);
	}

	public void OnDrawGizmos() {

		mover = this.GetComponent<ArtLineMover>();

	}

}
