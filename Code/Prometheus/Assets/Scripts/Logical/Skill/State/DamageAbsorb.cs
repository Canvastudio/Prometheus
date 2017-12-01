using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAbsorb : StateEffectIns
{
    EffectCondition condition;
    float absorb_damage;
    int times;

    public DamageAbsorb(LiveItem owner, StateConfig config, int index, PassiveSkillIns passive, LiveItem source) : base(owner, config, index, passive, source)
    {
        condition = config.stateArgs[index].ec[0];
        float[] f;
        absorb_damage = passive != null ? int.MaxValue : Rpn.CalculageRPN(config.stateArgs[index].rpn.ToArray(0), null, null, out f);
        stateType = StateEffectType.OnTakenDamage;
        times = passive != null ? int.MaxValue : Mathf.FloorToInt(config.stateArgs[index].f[0]);
    }

    protected override void Apply(object _damageInfo)
    {
        if (active)
        {
            times -= 1;

            var damageInfo = _damageInfo as Damage;

            damageInfo.damage = damageInfo.damage - absorb_damage;
            absorb_damage -= damageInfo.damage;

            if (damageInfo.damage < 0)
            {
                damageInfo.damage = 0;
            }

            if (absorb_damage <= 0 || times <= 0)
            {
                out_data = true;
            }
        }
    }
}
