using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E : MonoBehaviour {

    public Camera c;
    public Canvas ca;
    public RectTransform rt;
    float a = 1;

    // Update is called once per frame
    void Update () {

        bool inViewArea = false;

        int i = 10000;

        while (--i > 0)
        {
            var screen_Pos = RectTransformUtility.WorldToScreenPoint(c, transform.position);

            inViewArea = RectTransformUtility.RectangleContainsScreenPoint(rt
                ,
                screen_Pos,
                c);

            //a *= -1;

            //transform.Translate(Vector3.one * a);
        }
    }
}
