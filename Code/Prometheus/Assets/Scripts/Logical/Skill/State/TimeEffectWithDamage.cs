using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeEffectWithDamage : StateEffectIns
{
    [UnityEngine.SerializeField]
    DamageType dt;

    [UnityEngine.SerializeField]
    float interval;
    [UnityEngine.SerializeField]
    float t;

    public TimeEffectWithDamage(LiveItem owner, StateConfig config, int index, bool passive, LiveItem source) : base(owner, config, index, passive, source)
    {
        var ec = stateConfig.stateArgs[index].ec[0];
        dt = HelpFunction.EcToDamageType(ec);
        interval = stateConfig.stateArgs[index].f[0];
    }

    public override void OnTimeChange(float time)
    {
        base.OnTimeChange(time);

        if (active)
        {
            t += time;

            if (t > interval)
            {
                float[] v;
                float d = Rpn.CalculageRPN(stateConfig.stateArgs[index].rpn.ToArray(0), source, owner, out v, null, skillDamage);
                Damage damage = new Damage(d, source, owner, dt);
                owner.TakeDamage(damage);
            }
        }
    }
}
