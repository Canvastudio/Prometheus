using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MuiBase : MonoBehaviour {

    public abstract IEnumerator Init(object param);
    public abstract IEnumerator Open(object param);
    public abstract IEnumerator Close(object param);
    public abstract IEnumerator Hide(object param);
}
