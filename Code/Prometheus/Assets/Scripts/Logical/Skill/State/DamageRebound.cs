using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageRebound : StateIns
{
    EffectCondition condition;
    float percent = 0;

    public DamageRebound(LiveItem owner, StateConfig config, int index, bool passive)
        : base(owner, config, index, passive)
    {
        condition = config.stateArgs[index].ec[0];
        percent = config.stateArgs[index].f[0];
        stateType = StateEffectType.OnTakenDamage;
    }

    protected override void Apply(object _damageInfo)
    {
        var damageInfo = _damageInfo as Damage;
        if (!out_data)
        {
            if (FightComponet.CheckEffectCondition(condition, owner, damageInfo.damageType))
            {
                float damage = damageInfo.damage;
                float rebound_damage = damage * percent;
                damageInfo.damage = damage * (1 - percent);

                Damage di = new Damage(rebound_damage, owner, damageInfo.damageSource, damageInfo.damageType, false, true);
                damageInfo.damageSource.TakeDamage(di);
            }
        }
    }
}
