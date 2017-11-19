using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectImmune : StateEffectIns
{
    public SelectImmune(LiveItem owner, StateConfig config, int index, bool passive, LiveItem source) : base(owner, config, index, passive, source)
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
