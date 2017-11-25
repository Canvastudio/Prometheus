using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeSkillCost
{
    public RangeSkillCost(float _cost)
    {
        cost = _cost;
    }

    [UnityEngine.SerializeField]
    public float cost = 0;
}


public class Rapidly : StateEffectIns
{
    float extra;

    public Rapidly(LiveItem owner, StateConfig config, int index, bool passive, LiveItem source) : base(owner, config, index, passive, source)
    {
        extra = -stateConfig.stateArgs[index].f[0];

        stateType = StateEffectType.RangeSkillCost;
    }

    protected override void Apply(object param)
    {
        RangeSkillCost skillCost = param as RangeSkillCost;

        skillCost.cost = skillCost.cost * (1 + extra);
    }
}
