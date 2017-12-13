using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageStore : StateEffectIns
{

    /// <summary>
    /// 生效次数
    /// </summary>
    [UnityEngine.SerializeField]
    int store_times = 0;

    /// <summary>
    /// 返还倍率
    /// </summary>
    [UnityEngine.SerializeField]
    float multiply = 1;
    [UnityEngine.SerializeField]
    float total_store;
    [UnityEngine.SerializeField]
    EffectCondition condition;

    public DamageStore(LiveItem owner, StateConfig config, int index, PassiveSkillIns passive, LiveItem source) : base(owner, config, index, passive, source)
    {
        store_times = Mathf.FloorToInt(config.stateArgs[index].f[0]);
        multiply = config.stateArgs[index].f[1];
        condition = config.stateArgs[index].ec[0];
        stateType = StateEffectType.OnTakenDamage;
    }

    protected override void Apply(object _damageInfo)
    {
        var damageInfo = _damageInfo as Damage;
        if (!out_data && FightComponet.CheckEffectCondition(condition, owner, damageInfo.damageType))
        {
            store_times -= 1;

            total_store += damageInfo.damage;

            if (store_times == 0)
            {
                owner.AddHp(total_store * multiply);

                if (passive != null)
                {
                    store_times = Mathf.FloorToInt(stateConfig.stateArgs[index].f[0]);
                }
                else
                {
                    out_data = true;
                }

            }
        }
    }
}
