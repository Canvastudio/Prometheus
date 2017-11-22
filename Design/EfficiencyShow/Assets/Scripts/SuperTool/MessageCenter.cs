using System;
using System.Collections.Generic;
using System.Linq;


public class MessageCenter : SingleObject<MessageCenter>
{
    private readonly Dictionary<object, MsgHandle> eventDic = new Dictionary<object, MsgHandle>();
    private readonly Dictionary<object, MessageBulider> buliderDic = new Dictionary<object, MessageBulider>();
    public delegate void MsgHandle(object arg = null);
    protected override void Init() { }

    public int CheckEventCount()
    {
        return eventDic.Sum(v => v.Value.GetInvocationList().Length);
    }



    /// <summary>
    /// 创建一个MessageBulider对象，通过该方法创建的对象，可以在MessageCenter里管理
    /// </summary>
    public static MessageBulider CreatMsgBuilder(object mark)
    {
        if (mark == null) throw new ArgumentException("mark不能为空");
        if (Instance.buliderDic.ContainsKey(mark)) throw new ArgumentException("MessageBulider生成mark重复");
        var t = new MessageBulider();
        Instance.buliderDic.Add(mark, t);
        return t;
    }

    /// <summary>
    /// 移除一个MessageBulider对象，并移除它监听的消息
    /// </summary>
    public static void RemoveMsgBuilder(object mark)
    {
        if (!Instance.buliderDic.ContainsKey(mark)) throw new ArgumentException("MessageBulider移除失败");
        Instance.buliderDic[mark].RemoveAllListener();
        Instance.buliderDic.Remove(mark);
    }

    /// <summary>
    /// 添加监听者与对应的处理函数，为了能自动回收过期的消息，最好不要使用匿名或者静态方法
    /// </summary>
    /// <param name="t">关心的消息</param>
    /// <param name="m">消息处理函数，为了能自动回收过期的消息，最好不要使用匿名或者静态方法</param>
    public void AddListener(object t, MsgHandle m)
    {
        if (!(t is string || t is Enum) || t == null) throw new ArgumentException("监听类型只支持枚举与字符串，且不能为空");
        if (eventDic.ContainsKey(t))
        {
            MsgHandle handle = eventDic[t];
            Delegate[] delegates = handle.GetInvocationList();
            //添加前检查是否有重复
            if (delegates.Any(temp => temp == m)) return;
            handle += m;
            eventDic[t] = handle;
        }
        else
            eventDic.Add(t, m);
    }


    /// <summary>
    /// 移除监听者
    /// </summary>
    public void RemoveListener(object t, MsgHandle m)
    {
        if (eventDic.ContainsKey(t))
        {
            MsgHandle handle = eventDic[t];
            handle -= m;
            eventDic[t] = handle;
            if (handle == null) eventDic.Remove(t);
        }
        //else
        //    throw new ArgumentException("要移除的消息不存在/未注册！");
    }

    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="t">消息内容</param>
    /// <param name="arg">传递的参数，也可以不传，默认为空</param>
    public void PostMsg(object t, object arg = null)
    {
        if (eventDic.ContainsKey(t))
        {
            MsgHandle handle = eventDic[t];
            if (SuperTool.CheckDelegate(handle))
                handle(arg);
            else
                RemoveListener(t, handle);
        }
    }


}

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


