using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C : MonoBehaviour {

    public int[] array = new int[] { 1, 0, 1, 0, 1, 0 };

    public int CC()
    {
        int v = Random.Range(1, 61); //60是1,2,3,4,5,6的最小公倍数

        int m = 0;

        while (true)
        {
            if (array[m % array.Length] == 0)
            {
                v -= 1;

                if (v <= 0)
                    break;
            }

            ++m;
        }

        return m % array.Length;
    }

    public int[] res = new int[6];

    private void Start()
    {
        int i = 10000;

        while(i-- > 0)
        {
            res[CC()] += 1;
        }
    }
}
