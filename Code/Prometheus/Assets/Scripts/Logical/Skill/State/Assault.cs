using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assault : StateIns
{
    EffectCondition condition;
    float extra = 0;
    float total_extra = 0;

    public Assault(LiveItem owner, StateConfig config, int index, bool passive) : base(owner, config, index, passive)
    {
        condition = stateConfig.stateArgs[index].ec[0];
        extra = stateConfig.stateArgs[index].f[0];

        stateType = StateEffectType.OnGenerateDamage;
    }

    public override void Active()
    {
        base.Active();

        Messenger<Brick>.AddListener(SA.PlayerMoveStep, OnPlayerMove);
    }

    public override void Deactive()
    {
        base.Deactive();

        Messenger<Brick>.RemoveListener(SA.PlayerMoveStep, OnPlayerMove);
    }

    private void OnPlayerMove(Brick brick)
    {
        if (active)
        {
            total_extra += extra;
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
