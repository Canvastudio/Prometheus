using UnityEngine;
using System;

/// <summary>
/// 不要在外部使用这个类
/// </summary>
public class TimerTick
{
    private float leftTime;
    private readonly SuperTimer.TimerHandle callBack;
    private bool isDone;
    private readonly object arg;


    public TimerTick(float leftTime, SuperTimer.TimerHandle callBack, object arg)
    {
        this.leftTime = leftTime;
        this.callBack = callBack;
        isDone = false;
        this.arg = arg;
    }

    public bool IsDone()
    {
        return isDone;
    }

    public void Update()
    {
        if (!isDone)
        {
            leftTime -= SuperTimer.DeltaTime;
            if (leftTime <= 0)
            {
                try
                {
                    if (SuperTool.CheckDelegate(callBack))
                    {
                        callBack(arg);
                    }
                }
                catch (Exception)
                {
                    Debug.LogError("Timer回调异常：检查回调方法的所属对象是否为空");
                    throw;
                }
                isDone = true;
            }
        }
    }
}
