using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Silent : StateEffectIns
{
    public Silent(LiveItem owner, StateConfig config, int index, bool passive, LiveItem source) : base(owner, config, index, passive, source)
    {
    }

    public override void Active()
    {
        base.Active();

        owner.Silent = true;
    }

    public override void Deactive()
    {
        base.Deactive();

        owner.Silent = false;
    }

    protected override void Apply(object param)
    {
        throw new System.NotImplementedException();
    }
}
