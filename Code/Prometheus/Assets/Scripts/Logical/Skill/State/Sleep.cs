using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sleep : StateEffectIns
{
    public Sleep(LiveItem owner, StateConfig config, int index, PassiveSkillIns passive, LiveItem source) : base(owner, config, index, passive, source)
    {
    }


    public override void Active()
    {
        base.Active();

        owner.Sleep += 1;
    }

    public override void Deactive()
    {
        base.Deactive();

        owner.Sleep -= 1;
    }

    public override void Remove()
    {
        base.Remove();

        owner.Sleep -= 1;
    }

    protected override void Apply(object param)
    {
        throw new System.NotImplementedException();
    }
}
