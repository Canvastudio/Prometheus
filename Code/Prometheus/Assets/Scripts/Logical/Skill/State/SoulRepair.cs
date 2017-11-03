using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulRepair : StateIns
{
    EffectCondition condition;
    float restoration = 0;

    public SoulRepair(LiveItem owner, StateConfig config, int index, bool passive) : base(owner, config, index, passive)
    {
        condition = stateConfig.stateArgs[index].ec[0];
        restoration = stateConfig.stateArgs[index].f[0];

        Messenger<Damage>.AddListener(SA.MonsterDead, OnMonsterDead);
    }

    private void OnMonsterDead(Damage damage)
    {
        if (active && damage.damageType == DamageType.Physical && damage.damageSource.itemId == owner.itemId)
        {
            if (FightComponet.CheckEffectCondition(condition, damage.damageTarget, damage.damageType))
            {
                owner.cur_hp += owner.fmax_hp * restoration;
            }
        }
    }

    protected override void Apply(object param)
    {
        Debug.Log("SouldRepair 无需手动使用.");
    }
}
