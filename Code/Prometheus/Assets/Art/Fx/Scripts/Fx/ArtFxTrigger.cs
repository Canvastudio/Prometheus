using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtFxTrigger : MonoBehaviour {

	void OnEnable() {
		
		RuleBox.OpenBoxTrigger(transform.position);

	}

}
