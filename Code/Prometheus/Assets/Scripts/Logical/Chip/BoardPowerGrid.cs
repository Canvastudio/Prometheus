using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BoardPowerGrid : MonoBehaviour{

    public int id;

    /// <summary>
    /// 一个电网包含的电源，也就是这个网状啊结构遍历的起点
    /// </summary>
    public List<BoardInstanceBase> supplyList = new List<BoardInstanceBase>();

    /// <summary>
    /// 已经激活的芯片
    /// </summary>
    public Dictionary<int, List<BoardInstanceBase>> activeDic= new Dictionary<int, List<BoardInstanceBase>>();

    /// <summary>
    /// 连入电网 但是还没有激活的,索引表示深度 从0开始，0表示直接连接到电源
    /// </summary>
    public Dictionary<int, List<BoardInstanceBase>> unactiveDic = new Dictionary<int, List<BoardInstanceBase>>();

    /// <summary>
    /// 保存构建中的搜索状态
    /// </summary>
    public List<BoardInstanceBase> searchList = new List<BoardInstanceBase>();

    public void AddSupply(BoardInstanceBase supply)
    {
        powerGridTotalPower += supply.powerSupply;
        supplyList.Add(supply);
    }

    public void AddSupply(List<BoardInstanceBase> supplys)
    {
        foreach (var supply in supplys)
        {
            powerGridTotalPower += supply.powerSupply;
            supplyList.Add(supply);
            supply.powerGrid = this;
        }
    }

    /// <summary>
    /// 电网总共能提供的电量
    /// </summary>
    [SerializeField]
    private float _powerGridTotalPower = 0;
    public float powerGridTotalPower
    {
        get { return _powerGridTotalPower; }
        set
        {
            remainingDirty = true;
            _powerGridTotalPower = value;
        }
    }


    /// <summary>
    /// 当前电网总共消耗的电量
    /// </summary>
    [SerializeField]
    private float _powerGridCastPower = 0;
    public float powerGridCastPower
    {
        get { return _powerGridCastPower; }
        set
        {
            remainingDirty = true;
            _powerGridCastPower = value;
        }
    }


    public bool remainingDirty = false;

    private void OnPowerGridRefresh()
    {
        powerGridCastPower = 0;
    }

    void LateUpdate()
    {
        if (remainingDirty) remainingDirty = false;
    }

    public void Clean()
    {
        supplyList.Clear();
        activeDic.Clear();
        unactiveDic.Clear();
        searchList.Clear();
        _powerGridCastPower = 0;
        _powerGridTotalPower = 0;
    }
}
