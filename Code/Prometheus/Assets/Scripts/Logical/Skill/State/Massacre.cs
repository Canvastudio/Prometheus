using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Massacre : StateIns
{
    EffectCondition condition1;
    EffectCondition condition2;

    float extra = 0;
    float total_extra = 0;
    float max_extra = 0;

    public Massacre(LiveItem owner, StateConfig config, int index, bool passive) : base(owner, config, index, passive)
    {
        condition1 = stateConfig.stateArgs[index].ec[0];
        condition2 = stateConfig.stateArgs[index].ec[1];

        extra = stateConfig.stateArgs[index].f[0];
        max_extra = stateConfig.stateArgs[index].f[1];


        Messenger<Damage>.AddListener(SA.MonsterDead, OnMonsterDead);

        stateType = StateEffectType.OnGenerateDamage;
    }

    private void OnMonsterDead(Damage damage)
    {
        if (active && damage.damageType == DamageType.Physical && damage.damageSource.itemId == owner.itemId)
        {
            if (FightComponet.CheckEffectCondition(condition1, damage.damageTarget, damage.damageType))
            {
                total_extra = Mathf.Max(max_extra, total_extra + extra);
            }
        }
    }

    protected override void Apply(object param)
    {
        Damage damage = param as Damage;

        if (active && FightComponet.CheckEffectCondition(condition2, null, damage.damageType))
        {
            damage.damage = damage.damage * (1 + total_extra);
        }
    }
}
