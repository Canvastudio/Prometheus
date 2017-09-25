using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCore : SingleObject<StageCore> {

    Dictionary<ulong, Monster> monsterDic = new Dictionary<ulong, Monster>();

    ulong monsterId = 0;

    public void RegisterMonster(Monster newMonster)
    {
        newMonster.uid = monsterId++;

        monsterDic.Add(newMonster.uid, newMonster);
    }
}
