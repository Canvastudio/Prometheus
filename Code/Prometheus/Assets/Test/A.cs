using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A : MonoBehaviour {

    public bool go = false;

	// Use this for initialization
	void Start () {


        //yield return StartCoroutine(CA());
        SuperTimer.Instance.CoroutineStart(CA(), this);

    }
    
    
    // Update is called once per frame
    void Update () {
		
	}

    IEnumerator CA()
    {
        Debug.Log("A");
        yield return SuperTimer.Instance.CoroutineStart(CB(), this);
        Debug.Log("B");
        //yield return StartCoroutine(CB());
    }

    IEnumerator CB()
    {
        yield return SuperTimer.WaitTrue(check, go);
    }

    bool check(object go)
    {
        bool res = (bool)go;
        return res;
    }


}


