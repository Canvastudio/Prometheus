using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 存储怪物信息
/// </summary>
public class Monster : LiveItem
{
    /// <summary>
    /// 策划属性配置表
    /// </summary>
    public MonsterConfig config;

    /// <summary>
    /// 当前怪物强度
    /// </summary>
    public int pwr;
    /// <summary>
    /// 当前怪物等级
    /// </summary>
    public int lv;
    /// <summary>
    /// 当前怪物在表中的id
    /// </summary>
    public ulong cid;

    public override void OnDiscoverd()
    {
        base.OnDiscoverd();

        if (standBrick != null)
        {
            Debug.Log(gameObject.name);
            BrickCore.Instance.BlockNearbyBrick(standBrick.pathNode.x, standBrick.pathNode.z);
        }
    }

    public override void OnDead()
    {
        if (standBrick != null)
        {
            BrickCore.Instance.CancelBlockNearbyBrick(standBrick.pathNode.x, standBrick.pathNode.z);
        }
        else
        {
            Debug.LogError("怪物阵亡时发现: standbrick 为空");
        }

        Messenger<Monster>.Invoke(SA.MonsterDead, this);

        base.OnDead();
    }


    private void OnDisable()
    {

    }
}
