﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disarm : StateEffectIns
{
    public Disarm(LiveItem owner, StateConfig config, int index, bool passive) : base(owner, config, index, passive)
    {
    }

    public override void Active()
    {
        base.Active();

        owner.Disarm = true;
    }

    public override void Deactive()
    {
        base.Deactive();

        owner.Disarm = false;
    }

    protected override void Apply(object param)
    {
        throw new System.NotImplementedException();
    }
}