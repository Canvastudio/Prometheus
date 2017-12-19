using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disarm : StateEffectIns
{
    public Disarm(LiveItem owner, StateConfig config, int index, PassiveSkillIns passive, LiveItem source) : base(owner, config, index, passive, source)
    {
    }

    public override void Active()
    {
        base.Active();

        owner.Disarm += 1;
    }

    public override void Deactive()
    {
        base.Deactive();

        owner.Disarm -= 1;
    }


    public override void Remove()
    {
        base.Remove();

        owner.Disarm -= 1;
    }

    protected override void Apply(object param)
    {
        throw new System.NotImplementedException();
    }
}
