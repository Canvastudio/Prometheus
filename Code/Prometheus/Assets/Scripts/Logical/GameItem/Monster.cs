using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour{
     
    public MonsterProperty property;

    public ulong uid = 0;

    public Brick monsterBrick;

    /// <summary>
    /// 策划属性配置表
    /// </summary>
    public MonsterConfig config;

    /// <summary>
    /// 战斗组件
    /// </summary>
    public FightComponet fightComponet;

}
