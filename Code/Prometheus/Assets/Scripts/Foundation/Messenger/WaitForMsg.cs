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

    public WaitForMsgRes result = new WaitForMsgRes();

    private void SetPara<T>(string _msg, T _para)
    {
        result.para = _para;
        result.msg = _msg;

        finish_call = true;

        Messenger<T>.RemoveListener(_msg, SetPara);
    }

    public WaitForMsg BeginWaiting<T>(string msg)
    {
        finish_call = false;

        Messenger<T>.AddListener(msg, SetPara);

        return this;
    }
    public override bool keepWaiting
    {
        get
        {
            return !finish_call;
        }
    }
}
