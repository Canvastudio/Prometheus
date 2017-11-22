using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateDone : MonoBehaviour
{
    public void FallDone()
    {
        SendMessageUpwards("DoneFall");
    }

    public void QiPao()
    {
        SendMessageUpwards("ChangeState");
    }
}
