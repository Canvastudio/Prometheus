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

        base.Active();
    }

    public override void Deactive()
    {
        base.Deactive();

        Messenger.RemoveListener(SA.EnmeyCountChange, CheckEnemyCount);
    }


    private void CheckEnemyCount()
    {
        ResetChange();
        ApplyChange();
    }

    protected override void Apply(object param)
    {
        throw new System.NotImplementedException();
    }
}
