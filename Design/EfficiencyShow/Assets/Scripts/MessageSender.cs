using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageSender : MonoBehaviour
{

    public void ButtonClick(string msg)
    {
        if (!ClickLimit.AlowClick) return;
        MessageCenter.Instance.PostMsg(MSG_BT.Down);
        MessageCenter.Instance.PostMsg(msg);
    }
}


public enum MSG_BT
{
    Down,
}
