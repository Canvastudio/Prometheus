using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtTestSkill : MonoBehaviour {

	public List<Transform> tranlist;

	public string skill_name = "liudan";
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown(KeyCode.F)) {

			ArtSkill.DoSkill(skill_name, tranlist[0].position, tranlist[1].position, null);
		
		}
		
	}
}
