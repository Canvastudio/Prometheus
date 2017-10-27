using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B : MonoBehaviour {

    Dictionary<string, float> d = new Dictionary<string, float>();

	// Use this for initialization
	void Start () {
        CoroCore.Instance.ExStartCoroutine(Aciton());
	}
	
	// Update is called once per frame
	void Update () {
		

	}

    public int v = 0;

    private IEnumerator Aciton()
    {
        for (int i = 0; i < 5000; ++i)
        {
            d[i.ToString()] = 1;
            yield return wait;
            float ii = 1f;
            if (d.TryGetValue(i.ToString(), out ii))
            {
                v = v + 1;
            }
        }
    }

    WaitForSeconds wait = new WaitForSeconds(0.1f);
}
