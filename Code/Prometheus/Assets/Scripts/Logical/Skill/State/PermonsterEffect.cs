using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PermonsterEffect : Property
{
    private int enemyCount = 0;

    public PermonsterEffect(LiveItem owner, StateConfig config, int index, bool passive, LiveItem source) : base(owner, config, index, passive, source)
    {
        stateType = StateEffectType.PropertyChange;
    }

    public override void Active()
    {
        Messenger.AddListener(SA.EnmeyCountChange, CheckEnemyCount);
    }

    public override void Deactive()
    {
        base.Deactive();

        Messenger.RemoveListener(SA.EnmeyCountChange, CheckEnemyCount);
    }


    private void CheckEnemyCount()
    {
        int c = GContext.Instance.enemy_count;

        if (c != enemyCount)
        {
            ApplyChange();
            enemyCount = c;
        }
    }

    protected override void Apply(object param)
    {
        throw new System.NotImplementedException();
    }
}
