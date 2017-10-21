using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardInstanceBase : MonoBehaviour, IEquatable<BoardInstanceBase> {


    public int uid;
    [SerializeField]
    private bool _isPower;
    public bool isPower
    {
        get
        {
            return _isPower;
        }
        set
        {
#if UNITY_EDITOR

            OnSetPowerState(value);
#endif
            _isPower = value;
        }
    }

    protected virtual void OnSetPowerState(bool value)
    {

    }

    public int row = int.MinValue;
    public int col = int.MinValue;

    private int _depth = int.MaxValue;
    public int depth
    {
        get
        {
            return _depth;
        }
        set
        {
            SetDepth(value);
            _depth = value;
        }
    }

    protected virtual void SetDepth(int depth)
    {

    }

    /// <summary>
    /// 和负极它连接的节点
    /// </summary>
    public List<BoardInstanceBase> negativeConnectInstance = new List<BoardInstanceBase>();

    /// <summary>
    /// 和正极极它连接的节点
    /// </summary>
    public List<BoardInstanceBase> positiveConnectInstance = new List<BoardInstanceBase>();

    /// <summary>
    /// 实例所属的电网
    /// </summary>
    public BoardPowerGrid powerGrid;

    /// <summary>
    /// 表示接入一个电网之后，它在电网中的消耗
    /// </summary>
    public float castPower = 0;

    /// <summary>
    /// 表示接入电网后，提供的电量
    /// </summary>
    public float powerSupply = 0;

    private void OnEnable()
    {
        Messenger.AddListener(ChipBoardEvent.CheckPowerState, OnConstructPowerGrid);
    }

    private void OnConstructPowerGrid()
    {
        depth = int.MaxValue;
    }

    public bool Equals(BoardInstanceBase other)
    {
        return uid == other.uid;
    }
}
