using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtTestProperty : MonoBehaviour {

	// Use this for initialization
	void Start () {

		ArtSkill.Show("wake_up_monster", this.transform.position, Random.Range(1, 5));	
		
	}

}
