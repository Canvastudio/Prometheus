using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAuto : TempObj
{
    public bool fall;

    // Use this for initialization
    void Start()
    {
        ClickLimit.AddLock(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (!fall) return;
        gameObject.transform.Rotate(Vector3.forward,3);
    }

    public void DoneFall()
    {
        fall = true;
        ClickLimit.UnLock(this,true);
    }
}
