using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enhance : StateEffectIns
{
    float threshold = 1;
    EffectCondition condition;
    float extra = 0;


    public Enhance(LiveItem owner, StateConfig config, int index, bool passive, LiveItem source) : base(owner, config, index, passive, source)
    {
        threshold = stateConfig.stateArgs[index].f[0];
        extra = stateConfig.stateArgs[index].f[1];
        condition = stateConfig.stateArgs[index].ec[0];
        stateType = StateEffectType.OnGenerateDamage;
    }

    protected override void Apply(object _damageInfo)
    {
        var damageInfo = _damageInfo as Damage;
        float f = damageInfo.damageTarget.cur_hp / damageInfo.damageTarget.fmax_hp;
        if (threshold > f)
        {
            if (FightComponet.CheckEffectCondition(condition, owner, damageInfo.damageType))
            {
                damageInfo.damage = damageInfo.damage * (1 + extra);
            }
        }
    }
}
