using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtFxBase : MonoBehaviour {

	public Vector3 startPos;
	public Vector3 endPos;

	public Transform tran;
	public Callback OnHit = null;

	public float total_time = 2f;
	public float m_time = 0;

	public string fxend = "";

	public bool islookat = false;

	public bool iscenter = false;

	public Transform tran_start = null;
	public Transform tran_end = null;

	public virtual void Init(Vector3 _starPos, Vector3 _endPos, Callback _OnHit = null) {
	
		startPos = _starPos;
		endPos = _endPos;

		tran = this.transform;

		if (iscenter) {
		
			Vector3 center = Camera.main.transform.position;
			startPos.y = center.y;
			startPos.x = center.x;
		
		}

		tran.position = startPos;

		if(islookat)
			tran.rotation = ArtMath.LookAtZ(endPos - startPos);

		OnHit = _OnHit;
		m_time = 0;
	
	}

	public virtual void Init(Transform _tran_start, Transform _tran_end, Callback _OnHit = null) {

		tran_start = _tran_start;
		tran_end = _tran_end;

		tran = this.transform;
		tran_start.SetPos(ref startPos);

		if (iscenter) {

			Vector3 center = Camera.main.transform.position;
			startPos.y = center.y;
			startPos.x = center.x;

		}

		tran.position = startPos;
		endPos = startPos;

		tran_end.SetPos(ref endPos);

		OnHit = _OnHit;
		m_time = 0;

	}

	void Update() {
	
		m_time += Time.deltaTime;

		if (m_time > total_time)
			OnEnd();
		else
			UpdatePos();
	
	}

	public virtual void UpdatePos() {

		tran_end.SetPos(ref endPos);

		if(islookat)
			tran.rotation = ArtMath.LookAtZ(endPos - startPos);
	
	}

	public void OnEnd() {

		GameObject obj = FxPool.Get(FxEnum.Fx, fxend);

		if (obj != null)
			obj.GetComponent<ArtFxBase>().Init(this.tran, null);

		if (OnHit != null)
			OnHit();
	
		FxPool.Recover(this.gameObject);

	}


	#region set layer

	public string layername = "StageUI";

	public void SetLayer(string _layername) {
	
		layername = _layername;

		ParticleSystemRenderer[] prenders = this.GetComponentsInChildren<ParticleSystemRenderer>(true);

		for(int i = 0; i < prenders.Length; i++) {
		
			prenders[i].sortingLayerName = layername;
		
		}
	
	}

	public void OnDrawGizmos() {
	
		SetLayer("StageView");
	
	}

	#endregion

}
