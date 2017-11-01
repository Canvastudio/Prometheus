﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageResist : StateIns
{
    float probability = 0;
    EffectCondition condition;
    float damage_decrease = 0;


    public DamageResist(LiveItem owner, StateConfig stateConfig, int index, bool passive)
        : base(owner, stateConfig, index, passive)
    {
        probability = stateConfig.stateArgs[index].f[0];
        condition = stateConfig.stateArgs[index].ec[0];
        damage_decrease = stateConfig.stateArgs[index].f[1];
        stateType = StateEffectType.OnTakenDamage;
    }

    protected override void Apply(object _damageInfo)
    {
        var damageInfo = _damageInfo as Damage;
        float f = Random.Range(0f, 1f);
        if (probability >= f)
        {
            if (FightComponet.CheckEffectCondition(condition, owner, damageInfo.damageType))
            {
                damageInfo.damage = damageInfo.damage * (1 - damage_decrease);
            }
        }
    }
}