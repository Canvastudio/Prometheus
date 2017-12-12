using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Massacre : StateEffectIns
{
    [UnityEngine.SerializeField]
    EffectCondition condition1;
    [UnityEngine.SerializeField]
    EffectCondition condition2;

    [UnityEngine.SerializeField]
    float extra = 0;
    [UnityEngine.SerializeField]
    float total_extra = 0;
    [UnityEngine.SerializeField]
    float max_extra = 0;

    public Massacre(LiveItem owner, StateConfig config, int index, PassiveSkillIns passive, LiveItem source) : base(owner, config, index, passive, source)
    {
        condition1 = stateConfig.stateArgs[index].ec[0];
        condition2 = stateConfig.stateArgs[index].ec[1];

        extra = stateConfig.stateArgs[index].f[0];
        max_extra = stateConfig.stateArgs[index].f[1];



        stateType = StateEffectType.OnGenerateDamage;
    }

    public override void Active()
    {
        base.Active();

        Messenger<Damage>.AddListener(SA.MonsterDead, OnMonsterDead);
        Messenger<SkillInfo>.AddListener(SA.LiveUseSkill, OnPlayUseSkill);
    }

    public override void Deactive()
    {
        base.Deactive();

        Messenger<SkillInfo>.RemoveListener(SA.LiveUseSkill, OnPlayUseSkill);
        Messenger<Damage>.RemoveListener(SA.MonsterDead, OnMonsterDead);
    }

    public override void Remove()
    {
        Messenger<SkillInfo>.RemoveListener(SA.LiveUseSkill, OnPlayUseSkill);
        Messenger<Damage>.RemoveListener(SA.MonsterDead, OnMonsterDead);
    }

    private void OnPlayUseSkill(SkillInfo info)
    {
        if (source.itemId == owner.itemId)
        {
            if (FightComponet.CheckEffectCondition(condition2, null, info.config.damageType))
            {
                total_extra = 0;
            }
        }
    }

    private void OnMonsterDead(Damage damage)
    {
        if (active && damage.damageSource.itemId == owner.itemId)
        {
            if (FightComponet.CheckEffectCondition(condition1, damage.damageTarget, damage.damageType))
            {
                total_extra = Mathf.Min(max_extra, total_extra + extra);
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
