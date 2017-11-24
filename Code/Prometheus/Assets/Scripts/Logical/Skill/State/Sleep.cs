﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sleep : StateEffectIns
{
    public Sleep(LiveItem owner, StateConfig config, int index, bool passive, LiveItem source) : base(owner, config, index, passive, source)
    {
    }


    public override void Active()
    {
        base.Active();

        owner.Sleep = true;
    }

    public override void Deactive()
    {
        base.Deactive();

        owner.Sleep = false;
    }

    public override void Remove()
    {
        base.Remove();

        owner.Sleep = false;
    }

    protected override void Apply(object param)
    {
        throw new System.NotImplementedException();
    }
}
