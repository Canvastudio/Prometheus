using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vjua : StateEffectIns {

    float floor;

    float total_extra;

    EffectCondition condition;

    public Vjua(LiveItem owner, StateConfig config, int index, PassiveSkillIns passive, LiveItem source) : base(owner, config, index, passive, source)
    {
        stateType = StateEffectType.OnGenerateDamage;
        floor = stateConfig.stateArgs[index].f[0];
        total_extra = stateConfig.stateArgs[index].f[1];
        condition = stateConfig.stateArgs[index].ec[0];
    }

    protected override void Apply(object param)
    {
        Damage damage = param as Damage;

        float hp = damage.damageTarget.cur_hp;
        float mhp = damage.damageTarget.max_hp;

        if (hp / mhp < floor)
        {
            if (active && FightComponet.CheckEffectCondition(condition, null, damage.damageType))
            {
                damage.damage = damage.damage * (1 + total_extra);
            }
        }
    }
}
