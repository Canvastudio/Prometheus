using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 存储怪物信息
/// </summary>
public class Monster : LiveItem {

    public ulong uid = 0;

    /// <summary>
    /// 策划属性配置表
    /// </summary>
    public MonsterConfig config;
}
