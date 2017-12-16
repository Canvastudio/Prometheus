using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtNormalAttack : MonoBehaviour {

	public Animator anim;
	public string anim_name;

	public Transform tranAnim;

	public Vector3 driction;

	public Vector3 orignalPos;


	//test code
	public Transform t_a;
	public Transform t_b;

	// Use this for initialization
	void Start () {
		
		//DoAttack(t_a, t_b);

	}
	
	// Update is called once per frame
	void Update () {

		if(tranAnim != null) {
			tranAnim.localPosition = orignalPos + driction * this.transform.position.x;
			tranAnim.localScale = this.transform.localScale;
		}
		
	}

	public void DoAttack(Transform tranA, Transform tranB) {
		
		anim.Play(anim_name);

		tranAnim = tranA;

		driction = (tranB.position - tranA.position).normalized;

		orignalPos = tranA.localPosition;

	}

}
