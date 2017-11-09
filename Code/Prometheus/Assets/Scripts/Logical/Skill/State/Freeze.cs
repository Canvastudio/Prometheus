using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freeze : StateEffectIns
{
    public Freeze(LiveItem owner, StateConfig config, int index, bool passive) : base(owner, config, index, passive)
    {
    }

    protected override void Apply(object param)
    {
        throw new System.NotImplementedException();
    }

    public override void Active()
    {
        base.Active();

        owner.Freeze = true;
    }

    public override void Deactive()
    {
        base.Deactive();

        owner.Freeze = false;
    }
}
