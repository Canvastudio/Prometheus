using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBoost : StateEffectIns
{
    EffectCondition condition;
    float extra = 0;
    float total_extra;
    float max_extra;

    float last_time = 0;

    public DamageBoost(LiveItem owner, StateConfig config, int index, bool passive, LiveItem source) : base(owner, config, index, passive, source)
    {
        condition = stateConfig.stateArgs[index].ec[0];
        extra = stateConfig.stateArgs[index].f[0];
        max_extra = stateConfig.stateArgs[index].f[1];

        stateType = StateEffectType.OnGenerateDamage; 
    }

    public override void Active()
    {
        base.Active();

        Messenger<ActiveSkillsConfig>.AddListener(SA.PlayerUseSkill, OnPlayUseSkill);
    }

    public override void Deactive()
    {
        base.Deactive();

        Messenger<ActiveSkillsConfig>.RemoveListener(SA.PlayerUseSkill, OnPlayUseSkill);
    }
    private void OnPlayUseSkill(ActiveSkillsConfig config)
    {
        if (FightComponet.CheckEffectCondition(condition, null, config.damageType))
        {
            total_extra = 0;
        }
    }

    public override void OnTimeChange(float time)
    {
        base.OnTimeChange(time);

        if (exist_time - last_time >= stateConfig.interval)
        {
            last_time = exist_time;

            total_extra = Mathf.Max(max_extra, total_extra + extra);
        }
    }

    protected override void Apply(object param)
    {
        Damage damage = param as Damage;

        if (active && FightComponet.CheckEffectCondition(condition, null, damage.damageType))
        {
            damage.damage = damage.damage * (1 + total_extra);
        }
    }
}
