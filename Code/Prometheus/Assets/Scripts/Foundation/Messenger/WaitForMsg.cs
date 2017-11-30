using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 用在协程中等待并执行，执行完之后携程继续
/// </summary>
public class WaitForMsg : CustomYieldInstruction
{
    public class WaitForMsgRes
    {
        public System.Object para;

        public string msg;
    }

    private bool finish_call = false;

    private bool autoRemove = true;

    public WaitForMsgRes result = new WaitForMsgRes();

    List<Callback> OnFinish = new List<Callback>(4);

    private void SetPara<T>(string _msg, T _para)
    {
        result.para = _para;

        result.msg = _msg;

        finish_call = true;

        if (autoRemove)
        {
            foreach (var cb in OnFinish)
            {
                cb.Invoke();
            }

            OnFinish.Clear();
        }
    }

    public WaitForMsg StopWaiting<T>(string msg)
    {
        Messenger<T>.RemoveListener(msg, SetPara<T>);

        return this;
    }

    public WaitForMsg BeginWaiting<T>(string msg)
    {
        finish_call = false;

        Messenger<T>.AddListener(msg, SetPara<T>);

        OnFinish.Add(() =>
        {
            StopWaiting<T>(msg);
        });

        return this;
    }

    public void SetAutoRemove(bool b)
    {
        autoRemove = b;
    }

    public override bool keepWaiting
    {
        get
        {
            return !finish_call;
        }
    }
}
