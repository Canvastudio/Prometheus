using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freeze : StateEffectIns
{
    public Freeze(LiveItem owner, StateConfig config, int index, PassiveSkillIns passive, LiveItem source) : base(owner, config, index, passive, source)
    {
        Debug.Log("Frezz");
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

    public override void Remove()
    {
        base.Remove();

        owner.Freeze = false;
    }
}
