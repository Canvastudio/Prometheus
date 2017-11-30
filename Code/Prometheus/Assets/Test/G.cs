using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G : MonoBehaviour {

    public StateEffectIns[] a;
    System.Delegate d1;

    // Use this for initialization
	void Awake () {
        a = new StateEffectIns[2];

        d1 = (Callback)d1 + dd;
        d1 = (Callback)d1 + dd;

        var cb = d1 as Callback;
        cb.Invoke();
    }


    public void dd()
    {
        Debug.Log("123");
    }

    //private void Update()
    //{

    //    Debug.Log(Random.Range(0, a.Length));
    //}
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
