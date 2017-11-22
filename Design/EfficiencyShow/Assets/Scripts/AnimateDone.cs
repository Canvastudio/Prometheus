using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateDone : MonoBehaviour
{
    public void Done()
    {
        SendMessageUpwards("FallDone");
    }
}
