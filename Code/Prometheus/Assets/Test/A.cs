using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A : MonoBehaviour {

    public bool go = false;
    public bool stop = false;

    Coroutine coroutine;

	// Use this for initialization
	void Start () {


        //yield return StartCoroutine(CA());
        //SuperTimer.Instance.CoroutineStart(CA(), this);
        coroutine = TCoroCore.Instance.StartCoro(CA());

        Debug.Log("end");
    }
    
    
    // Update is called once per frame
    void Update () {
		
        if (stop) TCoroCore.Instance.StopCoro(b);
	}

    IEnumerator b;
    IEnumerator CA()
    {
        Debug.Log("A");
        //yield return SuperTimer.Instance.CoroutineStart(CB(), this);
        b = TCoroCore.Instance.StartInnerCoro(CB());
        yield return StartCoroutine(b);
        Debug.Log("B");

    }

    IEnumerator CB()
    {
        yield return new WaitUntil(() => go);

        while (true)
        {
            Debug.Log("C");

            yield return 0;
        }
    }

    bool check(object go)
    {
        bool res = (bool)go;
        return res;
    }


}


