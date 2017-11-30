using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Critical : StateEffectIns
{
    [UnityEngine.SerializeField]
    float probability = 0;
    [UnityEngine.SerializeField]
    float multiply = 1;
    [UnityEngine.SerializeField]
    EffectCondition condition;

    public Critical(LiveItem owner, StateConfig config, int index, PassiveSkillIns passive, LiveItem source) : base(owner, config, index, passive, source)
    {
        probability = stateConfig.stateArgs[index].f[0];
        multiply = stateConfig.stateArgs[index].f[1];
        condition = stateConfig.stateArgs[index].ec[0];
        stateType = StateEffectType.OnGenerateDamage;
    }

    protected override void Apply(object _damageInfo)
    {
        var damageInfo = _damageInfo as Damage;
        float f = Random.Range(0f, 1f);
        if (probability >= f)
        {
            if (FightComponet.CheckEffectCondition(condition, owner, damageInfo.damageType))
            {
                damageInfo.damage = damageInfo.damage * multiply;
            }
        }
    }
}
