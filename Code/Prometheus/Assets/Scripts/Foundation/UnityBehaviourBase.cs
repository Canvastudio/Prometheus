using UnityEngine;
using System.Collections;

public class UnityBehaviourBase : MonoBehaviour {

	public Transform tran;
	public GameObject obj;
	//public int Side;
	public int Layer;
	public int Weight = 10;
	
	void Awake() {
	
		tran = this.transform;
		Layer = this.gameObject.layer;
		InitAwake();
	
	}

	public virtual void InitAwake() {}
	
	public virtual void Init() {}
	
	//public virtual void Init(params object[] objs) {}

	//public virtual void Init(params int[] objs) {}

	// Update is called once per frame
	void Update () {

		OutPauseUpdate();
	}
	
	public virtual void BaseUpdate() {}
	
	public virtual void OutPauseUpdate() {}
	
}
