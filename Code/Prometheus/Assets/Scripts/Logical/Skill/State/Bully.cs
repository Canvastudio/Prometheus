using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bully : StateEffectIns
{
    [UnityEngine.SerializeField]
    float threshold = 1;
    [UnityEngine.SerializeField]
    EffectCondition condition;
    [UnityEngine.SerializeField]
    float extra = 0;

    public Bully(LiveItem owner, StateConfig config, int index, bool passive, LiveItem source) : base(owner, config, index, passive, source)
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
        if (threshold < f)
        {
            if (FightComponet.CheckEffectCondition(condition, owner, damageInfo.damageType))
            {
                damageInfo.damage = damageInfo.damage * (1 + extra);
            }
        }
    }
}
