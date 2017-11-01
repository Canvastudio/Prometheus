﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Critical : DamageState
{
    float probability = 0;
    float multiply = 1;
    EffectCondition condition;

    public Critical(LiveItem owner, StateConfig config, int index, bool passive) : base(owner, config, index, passive)
    {
        probability = stateConfig.stateArgs[index].f[0];
        multiply = stateConfig.stateArgs[index].f[1];
        condition = stateConfig.stateArgs[index].ec[0];
    }

    protected override IEnumerator Apply(Damage damageInfo)
    {
        float f = Random.Range(0f, 1f);
        if (probability >= f)
        {
            if (FightComponet.CheckEffectCondition(condition, owner, damageInfo.damageType))
            {
                damageInfo.damage = damageInfo.damage * multiply;
            }
        }

        return null; ;
    }
}