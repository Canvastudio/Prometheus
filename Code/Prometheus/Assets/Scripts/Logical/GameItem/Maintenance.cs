using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maintenance : GameItemBase{

    public override void Recycle()
    {
        base.Recycle();

        GameObject.Destroy(this.gameObject);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
