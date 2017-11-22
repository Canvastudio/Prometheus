using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DriftTxt : MonoBehaviour
{
    public string[] content;

    void Awake()
    {
        var t = SuperTool.GetComponentsInChildren<Text>(this)[0];
        t.text = SuperTool.RandomElement(content);
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
