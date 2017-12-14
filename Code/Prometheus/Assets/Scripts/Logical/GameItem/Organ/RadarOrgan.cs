using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarOrgan : OrganBase
{
    public int openCount = 0;

    public override void Reactive()
    {
        var monsters =StageCore.Instance.tagMgr.GetEntity<Monster>(ETag.GetETag(ST.MONSTER, ST.UNDISCOVER));

        while (openCount > 0 && monsters.Count > 0)
        {
            int index = Random.Range(0, monsters.Count);
            var monster = monsters[index];
            monsters.RemoveAt(index);
            if (monster.standBrick.loopFx == null)
            {
                monster.standBrick.ShowTipDanger();
                --openCount;
            }
        }
    }
}
