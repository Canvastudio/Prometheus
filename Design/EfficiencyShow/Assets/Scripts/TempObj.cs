using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempObj : MonoBehaviour
{

    void Awake()
    {
        MessageCenter.Instance.AddListener(MSG_BT.Down, DestroySelf);
        Init();
    }

     protected virtual void Init() { }

    void Destroy()
    {
        MessageCenter.Instance.RemoveListener(MSG_BT.Down, DestroySelf);
    }

    void DestroySelf(object arg)
    {
        if (this != null) Destroy(gameObject);
    }

}
