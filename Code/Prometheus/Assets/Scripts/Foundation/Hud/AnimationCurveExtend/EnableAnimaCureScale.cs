using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableAnimaCureScale : AnimCurveScale {

	public void OnEnable() {
	
		this.transform.localScale = Vector3.zero;
		OnStart();
	
	}

}
