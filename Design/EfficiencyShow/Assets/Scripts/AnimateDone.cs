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

    public void BaoZa1()
    {
        SendMessageUpwards("HideSelf");
    }

    public void BaoZa2()
    {
        SendMessageUpwards("DestroySelf");
    }

    public void VffaMax()
    {
        SendMessageUpwards("_VffaMax");
    }

    public void VffaMin()
    {
        SendMessageUpwards("_VffaMin");
    }
}
