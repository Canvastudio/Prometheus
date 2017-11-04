using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectImmune : StateEffectIns
{
    public SelectImmune(LiveItem owner, StateConfig config, int index, bool passive) : base(owner, config, index, passive)
    {
        stateType = StateEffectType.SelectImmune;
    }

    public override void Active()
    {
        base.Active();
    }

    public override void Deactive()
    {
        base.Deactive();
    }


    protected override void Apply(object param)
    {
        throw new System.NotImplementedException();
    }
}
