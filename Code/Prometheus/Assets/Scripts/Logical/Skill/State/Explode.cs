﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : StateIns
{
    public Explode(LiveItem owner, StateConfig config, int index, bool passive) : base(owner, config, index, passive)
    {
        stateType = StateEffectType.Countdown;
    }

    protected override void Apply(object param)
    {
        Debug.LogError("Explode 这个状态是自动触发.");
    }

    public override void OnOutData()
    {
        base.OnOutData();

        GameProperty property;

        float damage = FightComponet.CalculageRPN(stateConfig.stateArgs[index].rpn.ToArray(0), owner, StageCore.Instance.Player, out property);

        Damage damageInfo = new Damage(damage, owner, StageCore.Instance.Player, DamageType.Physical);

        StageCore.Instance.Player.MeleeAttackByOther(owner, damageInfo);
    }
}
