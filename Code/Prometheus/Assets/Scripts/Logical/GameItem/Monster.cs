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

    public ulong uid = 0;

    /// <summary>
    /// 策划属性配置表
    /// </summary>
    public MonsterConfig config;

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
        base.OnDead();

        if (standBrick != null)
        {
            BrickCore.Instance.CancelBlockNearbyBrick(standBrick.pathNode.x, standBrick.pathNode.z);
        }
        else
        {
            Debug.LogError("怪物阵亡时发现: standbrick 为空");
        }
    }


    private void OnDisable()
    {

    }
}
