using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Silent : StateEffectIns
{
    public Silent(LiveItem owner, StateConfig config, int index, PassiveSkillIns passive, LiveItem source) : base(owner, config, index, passive, source)
    {
    }

    public override void Active()
    {
        base.Active();

        owner.Silent += 1;
    }

    public override void Deactive()
    {
        base.Deactive();

        owner.Silent -= 1;
    }

    protected override void Apply(object param)
    {
        throw new System.NotImplementedException();
    }
}
