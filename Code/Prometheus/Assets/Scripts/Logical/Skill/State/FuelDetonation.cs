using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelDetonation : StateEffectIns
{
    public EffectCondition condition;
    public float distance;
    public float additive;

    public FuelDetonation(LiveItem owner, StateConfig config, int index, PassiveSkillIns passive, LiveItem source) : base(owner, config, index, passive, source)
    {
        condition = stateConfig.stateArgs[index].ec[0];
        distance = stateConfig.stateArgs[index].f[0];
        additive = stateConfig.stateArgs[index].f[1];

        stateType = StateEffectType.OnGenerateDamage;
    }

    protected override void Apply(object param)
    {
        Damage damage = param as Damage;

        if (active && FightComponet.CheckEffectCondition(condition, null, damage.damageType))
        {
            if (owner.standBrick.pathNode.Distance(damage.damageTarget.standBrick.pathNode) <= distance)
            {
                damage.damage *= (1 + additive);
            }
        }
    }
}
