using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : StateEffectIns
{
    public Explode(LiveItem owner, StateConfig config, int index, PassiveSkillIns passive, LiveItem source) : base(owner, config, index, passive, source)
    {
        stateType = StateEffectType.Countdown;
        stateConfig = config;
    }

    protected override void Apply(object param)
    {
        Debug.LogError("Explode 这个状态是自动触发.");
    }

    public override void OnOutData()
    {
        if (active)
        {
            Debug.Log("自爆! " + owner.name);

            base.OnOutData();

            float[] f;

            var rpn = stateConfig.stateArgs[index].rpn;

            float damage = Rpn.CalculageRPN(rpn.ToArray(0), owner, StageCore.Instance.Player, out f);

            Damage damageInfo = new Damage(damage, owner, StageCore.Instance.Player, DamageType.Physical);

            CoroCore.Instance.StartCoroutine(StageCore.Instance.Player.MeleeAttackByOther(owner, damageInfo));

            Damage d = new Damage(999999, owner, owner, DamageType.Physical);

            //TODO:自爆特效

            owner.StartCoroutine(owner.OnDead(d));
        }
    }
}
