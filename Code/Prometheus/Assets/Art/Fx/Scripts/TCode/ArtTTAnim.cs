using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtTTAnim : MonoBehaviour {

	public Animator m_anim;
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKeyDown(KeyCode.A))
			m_anim.Play("anim_atk");

		if(Input.GetKeyDown(KeyCode.S))
			m_anim.Play("anim_hp");

		if(Input.GetKeyDown(KeyCode.W))
			m_anim.Play("anim_hudun");

		if(Input.GetKeyDown(KeyCode.E))
			m_anim.Play("anim_hujia");		
	}
}
