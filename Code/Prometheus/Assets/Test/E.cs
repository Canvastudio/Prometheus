using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E : MonoBehaviour
{
    public Camera c;
    public RectTransform rt;

    IEnumerator TestCoro()
    {
        Debug.Log("A" + (Time.frameCount - i));

        IEnumerator ie = TestCoro1();

        yield return ie;

        yield return TestCoro2();
    }

    IEnumerator TestCoro1()
    {
        Debug.Log("B" + (Time.frameCount - i));


        return TestCoro3();
    
    }

    IEnumerator TestCoro2()
    {
        Debug.Log("D" + (Time.frameCount - i));


        yield return TestCoro3();

    }

    IEnumerator TestCoro3()
    {
        Debug.Log("E" + (Time.frameCount - i));


        return null;
    }

    int i = 0;

    private void Start()
    {
        i = Time.frameCount;
        this.ExStartCoroutine(ooooo());
        Debug.Log("C" + (Time.frameCount - i));
    }

    private IEnumerator ooooo()
    {
        yield return TestCoro();

        Debug.Log("mid: " + (Time.frameCount - i).ToString());

        yield return 0;

        Debug.Log("final: " + (Time.frameCount - i).ToString());
    }
}
