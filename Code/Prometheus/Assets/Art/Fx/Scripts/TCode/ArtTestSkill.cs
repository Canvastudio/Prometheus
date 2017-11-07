using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtTestSkill : MonoBehaviour {

	public List<Transform> tranlist;

	public string skill_name = "liudan";

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown(KeyCode.F)) {
		
			GameObject objskill = FxPool.Get(FxEnum.Skill, skill_name);
			objskill.GetComponent<ArtEventFlow>().Init(tranlist[0].position, tranlist[1].position, null);
		
		}
		
	}
}
