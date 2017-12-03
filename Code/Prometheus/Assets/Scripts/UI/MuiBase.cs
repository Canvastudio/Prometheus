using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MuiBase : MonoBehaviour {

    Canvas canvas;

    public bool isHide = false;

    public virtual void Awake()
    {
        if (canvas == null)
        {
            canvas = GetComponent<Canvas>();
        }

        if (canvas.worldCamera == null)
        {
            canvas.worldCamera = GameManager.Instance.UiCamera;
        }
    }

    public abstract IEnumerator Init(object param = null);
    public abstract IEnumerator Open(object param = null);
    public abstract IEnumerator Close(object param = null);
    public abstract void Hide(object param = null);
}
