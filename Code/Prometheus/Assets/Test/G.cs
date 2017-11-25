﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G : MonoBehaviour {

    public StateEffectIns[] a;
    // Use this for initialization
	void Start () {
        co = StartCoroutine(Go());
    }

    IEnumerator ie;
    Coroutine co;

    IEnumerator Go()
    {
        yield return A();
    }

    IEnumerator A()
    {
        ie = B();

        yield return ie;

        Debug.Log(Time.frameCount);
    }

    IEnumerator B()
    {
        while (true)
        {
            Debug.Log("B:" + Time.frameCount);

            yield return new WaitForSeconds(1f);
        }
    }

    public void DoLoop()
    {
        
    }

    public void Stop()
    {
        StopCoroutine(ie);
    }
}
