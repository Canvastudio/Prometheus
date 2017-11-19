using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bang : StateEffectIns
{
    EffectCondition condition;
    float probability = 0;
    ulong state_id;

    public Bang(LiveItem owner, StateConfig config, int index, bool passive, LiveItem source) : base(owner, config, index, passive, source)
    {
        condition = stateConfig.stateArgs[index].ec[0];
        stateType = StateEffectType.OnGenerateDamage;
        probability = stateConfig.stateArgs[index].f[0];
        state_id = stateConfig.stateArgs[index].u[0];
    }

    protected override void Apply(object param)
    {
        Damage damage = param as Damage;

        if (active && FightComponet.CheckEffectCondition(condition, null, damage.damageType))
        {
            damage.attach_state = state_id;
        }
    }
}
