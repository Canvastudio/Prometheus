using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MuiBase : MonoBehaviour {

    public bool isHide = false;

    public abstract IEnumerator Init(object param = null);
    public abstract IEnumerator Open(object param = null);
    public abstract IEnumerator Close(object param = null);
    public abstract void Hide(object param = null);
}
