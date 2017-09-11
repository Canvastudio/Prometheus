using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 多消息集中管理器，方便消息集中管理与移除
/// </summary>
public class MessageBulider
{
    private readonly List<XY<object, MessageCenter.MsgHandle>> msgList;

    public MessageBulider()
    {
        msgList = new List<XY<object, MessageCenter.MsgHandle>>();
    }
    /// <summary>
    /// 注册一个消息，为了能自动回收过期的消息，最好不要使用匿名或者静态方法
    /// </summary>
    public void AddListener(object t, MessageCenter.MsgHandle m)
    {
        XY<object, MessageCenter.MsgHandle> msg = new XY<object, MessageCenter.MsgHandle>(t, m);
        if (!msgList.Contains(msg))
        {
            msgList.Add(msg);
            MessageCenter.Instance.AddListener(t, m);
        }
    }
    /// <summary>
    /// 移除所有本对象管理消息
    /// </summary>
    public void RemoveAllListener()
    {
        foreach (var v in msgList) MessageCenter.Instance.RemoveListener(v.x, v.y);
        msgList.Clear();
    }

}
