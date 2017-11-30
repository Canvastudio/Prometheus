using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAbsorb : StateEffectIns
{
    [UnityEngine.SerializeField]
    EffectCondition condition;
    [UnityEngine.SerializeField]
    float absorb_damage;
    [UnityEngine.SerializeField]
    int times;

    public DamageAbsorb(LiveItem owner, StateConfig config, int index, PassiveSkillIns passive, LiveItem source) : base(owner, config, index, passive, source)
    {
        condition = config.stateArgs[index].ec[0];
        float[] f;
        absorb_damage = passive != null ? -1f : Rpn.CalculageRPN(config.stateArgs[index].rpn.ToArray(0), null, null, out f);
        stateType = StateEffectType.OnTakenDamage;
        times = passive != null ? -1 : Mathf.FloorToInt(config.stateArgs[index].f[0]);
    }

    protected override void Apply(object _damageInfo)
    {
        var damageInfo = _damageInfo as Damage;
        damageInfo.damage = damageInfo.damage - absorb_damage;

        if (passive == null)
        {
            absorb_damage = Mathf.Max(0, -damageInfo.damage);
            times -= 1;
        }

        damageInfo.damage = Mathf.Max(0, damageInfo.damage);

        if (absorb_damage == 0 || times == 0)
        {
            out_data = true;
        }
    }
}
