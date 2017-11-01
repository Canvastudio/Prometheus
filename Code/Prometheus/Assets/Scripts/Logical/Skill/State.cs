using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum StateEffectType
{
    TakenDamage,
    AttackRelate,
    Invalid,
}

/// <summary>
/// 结算类的状态，比如伤害结算的减少伤害的， 攻击结算的造成暴击的
/// </summary>
public abstract class DamageState : IEquatable<DamageState>
{
    public StateEffectType stateType = StateEffectType.Invalid;
    public StateConfig stateConfig;

    public int index;
    protected bool passive;
    public LiveItem owner;

    private bool _out_data = false;
    public bool out_data
    {
        get { return _out_data; }
        set { if (value) OnOutData(); out_data = value; }
    }

    public DamageState(LiveItem owner, StateConfig config, int index, bool passive)
    {
        this.owner = owner;
        this.stateConfig = config;
        this.index = index;
        this.passive = passive;
    }

    protected abstract IEnumerator Apply(Damage damageInfo);

    public virtual void OnOutData()
    {
        stateConfig = null;

        owner.RemoveDefendState(this);

        owner = null;
    }

    public bool Equals(DamageState other)
    {
        return (stateConfig.id == other.stateConfig.id)
            && (index == other.index);
    }

    public IEnumerator ApplyState(object param)
    {
        IEnumerator ie = null;

        if (!owner.Silent)
        {
            switch (stateType)
            {
                case StateEffectType.TakenDamage:
                    ie = Apply(param as Damage);
                    break;
            }

            if (ie != null)
            {
                yield return ie;
            }
        }
    }

    public static void GenerateStateEffects(StateConfig config, LiveItem owner, bool passive, out DamageState[] effectIns)
    {
        var effects = config.stateEffects.ToArray();
        effectIns = new DamageState[effects.Length];

        for (int i = 0; i < effects.Length; ++i)
        {
            switch(effects[i])
            {
                case StateEffect.DamageAbsorb:
                    effectIns[i] = new DamageAbsorb(owner, config, i, passive);
                    break;
                case StateEffect.DamageRebound:
                    effectIns[i] = new DamageRebound(owner, config, i, passive);
                    break;
                case StateEffect.DamageResist:
                    effectIns[i] = new DamageResist(owner, config, i, passive);
                    break;
                case StateEffect.DamageStore:
                    effectIns[i] = new DamageStore(owner, config, i, passive);
                    break;
                case StateEffect.DamageTransfer:
                    effectIns[i] = new DamageTransfer(owner, config, i, passive);
                    break;
            }
        }
    }
}
