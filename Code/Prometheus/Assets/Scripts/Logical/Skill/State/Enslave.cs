using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enslave : StateEffectIns
{
    public Enslave(LiveItem owner, StateConfig config, int index, bool passive, LiveItem source) : base(owner, config, index, passive, source)
    {

    }

    public override void Active()
    {
        base.Active();

        (owner as Monster).enslave = true;
    }

    public override void Deactive()
    {
        (owner as Monster).enslave = false;
    }

    public override void Remove()
    {
        base.Remove();

        (owner as Monster).enslave = false;
    }
}
