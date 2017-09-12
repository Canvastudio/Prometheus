using UnityEngine;
using System.Collections;

public class GameBase : UnityBehaviourBase {
	
	public int ObjectId = 0;
	//[HideInInspector]
	public bool IsSleep = true;
	[HideInInspector]
	public Vector3 recoverPos;
	public string mlayer;
	public int tableid = 0;
	public int level;

	public void ZeroPosition() {
	
		tran.localPosition = Vector3.zero;
		tran.localEulerAngles = Vector3.zero;
		tran.localScale = Vector3.one;
	
	}

	public override void BaseUpdate ()
	{
		base.BaseUpdate ();
		if(!IsSleep) { Action(); }
	
	}

	public virtual void Action() { }

	public virtual void OutPool() { IsSleep = false; }

	public virtual void Recover() { IsSleep = true; }

	public virtual void OnGround() { IsSleep = true; }

	public virtual void RecoverPos() {

		tran.parent = null;
		tran.position = recoverPos; 
		Recover();

	}

	public virtual void Remove() {
		
		if(this != null)
			GameObject.Destroy (this.gameObject);
		
	}
	
	public virtual void SetLayer(string _layer) {
		
		mlayer = _layer;
		
	}
}
