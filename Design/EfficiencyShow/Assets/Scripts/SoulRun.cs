using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulRun : MonoBehaviour
{
    public void SetTarget(Transform tar)
    {
        var arg = MoveArgs.CreateMoveArg(gameObject, tar, 5);
        arg.acceleration = 5;
        arg.rotateAddSpeed = 5;
        arg.rotateAddSpeedType = MoveArgs.AccelerationType.Squa;
        arg.arriveDistance = 0.5f;
        arg.rotateSpeed = 40;
        arg.rotateAddSpeed = 20;
        arg.callback = o =>
         {
             Destroy(gameObject);
         };
        SuperTool.MoveTo2D(arg);
    }

}
