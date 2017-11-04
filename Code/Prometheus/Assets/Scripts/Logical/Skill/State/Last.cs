using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Last : StateEffectIns
{
    public Last(LiveItem owner, StateConfig config, int index, bool passive) : base(owner, config, index, passive)
    {
        stateType = StateEffectType.Last;
    }

    protected override void Apply(object param)
    {
        throw new System.NotImplementedException();
    }

    public override void Active()
    {
        base.Active();

        StageCore.Instance.tagMgr.SetEntityTag(owner, ETag.Tag(ST.LAST), true);
    }

    public override void Deactive()
    {
        base.Deactive();

        StageCore.Instance.tagMgr.SetEntityTag(owner, ETag.Tag(ST.LAST), false);
    }
}
