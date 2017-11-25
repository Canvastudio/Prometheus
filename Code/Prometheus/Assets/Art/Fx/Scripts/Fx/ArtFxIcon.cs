using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtFxIcon : ArtFxBase {

	public Texture tex;

	public override void Init(Transform _tran_start, Transform _tran_end, Callback _OnHit)
	{
		base.Init(_tran_start, _tran_end, _OnHit);

		ParticleSystemRenderer render = this.GetComponent<ParticleSystemRenderer>();
		render.material.mainTexture = tex;

	}

}
