using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B : MonoBehaviour {

    [SerializeField]
    GameObject gameObject;

    public class BB
    {
        public string bbbb = "1";
    }

    Dictionary<string, float> d = new Dictionary<string, float>();

    System.Action<BB> action;
	// Use this for initialization
	void Start () {

        GameObject.Instantiate(gameObject, this.transform).SetActive(true);
    }

    void GGGG(object g)
    {
        float gg = (float)g;

    }

    IEnumerator www()
    {
        yield return new WaitForSeconds(5);
    }
    void AAA(BB aa)
    {


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
