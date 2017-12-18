using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockMask : MonoBehaviour {

    [SerializeField]
    Button button;
    [SerializeField]
    AnimationCurve curve;

    public void Awake()
    {
        HudEvent.Get(button).onClick = OnClick;
    }

    private void OnClick()
    {
        StartCoroutine(DoAnimation());
    }

    IEnumerator DoAnimation()
    {
        float time = 0;
        while (true)
        {
            if (time > curve.keys[curve.length - 1].time)
            {
                time = 0;
                yield break;
            }
            else
            {
                transform.localScale = Vector3.one * curve.Evaluate(time);
                yield return 0;
                time += Time.deltaTime;
            }
        }
    }
}
