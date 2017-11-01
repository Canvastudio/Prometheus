﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageStore : DamageState
{

    /// <summary>
    /// 生效次数
    /// </summary>
    int store_times = 0;

    /// <summary>
    /// 返还倍率
    /// </summary>
    float multiply = 1;

    float total_store;

    EffectCondition condition;

    public DamageStore(LiveItem owner, StateConfig config, int index, bool passive) 
        : base (owner, config, index, passive)
    {
        store_times = Mathf.FloorToInt(config.stateArgs[index].f[0]);
        multiply = config.stateArgs[index].f[1];
        condition = config.stateArgs[index].ec[0];
        stateType = StateEffectType.TakenDamage;
    }

    protected override IEnumerator Apply(Damage damageInfo)
    {
        if (!out_data && FightComponet.CheckEffectCondition(condition, owner, damageInfo.damageType))
        {
            store_times -= 1;

            total_store += damageInfo.damage;

            if (store_times == 0)
            {
                owner.AddHp(total_store * multiply);

                if (passive)
                {
                    store_times = Mathf.FloorToInt(stateConfig.stateArgs[index].f[0]);
                }
                else
                {
                    out_data = true;
                }
                
            }

            damageInfo.damage = 0;
        }

        return null;
    }

    public override void OnOutData()
    {

    }
}
