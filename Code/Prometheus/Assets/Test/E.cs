using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E : MonoBehaviour
{
    public Camera c;
    public RectTransform rt;
    public UnityEngine.UI.Image i;

    // Update is called once per frame
    void Update()
    {

        bool inViewArea = false;

        var screen_Pos = RectTransformUtility.WorldToScreenPoint(c, transform.position);

        inViewArea = RectTransformUtility.RectangleContainsScreenPoint(rt
            ,
            screen_Pos,
            c);

        if (inViewArea)
        {
            i.color = Color.white;
        }
        else
        {
            i.color = Color.red;
        }

    }
}
