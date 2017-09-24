using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster {
     
    public MonsterProperty property;

    public ulong uid = 0;

    public Brick monsterBrick;

    /// <summary>
    /// 策划属性配置表
    /// </summary>
    public MonsterConfig config;

    public FightComponet fightComponet;

}
