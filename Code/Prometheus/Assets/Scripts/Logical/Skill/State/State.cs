using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum StateEffectType
{
    OnTakenDamage,
    OnGenerateDamage,
    Invalid,
}

/// <summary>
/// 结算类的状态，比如伤害结算的减少伤害的， 攻击结算的造成暴击的
/// </summary>
public abstract class StateIns : IEquatable<StateIns>
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

    public StateIns(LiveItem owner, StateConfig config, int index, bool passive)
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

    public bool Equals(StateIns other)
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
                case StateEffectType.OnTakenDamage:
                    ie = Apply(param as Damage);
                    break;
            }

            if (ie != null)
            {
                yield return ie;
            }
        }
    }

    public static StateIns GenerateStateEffects(StateConfig config, int i, LiveItem owner, bool passive)
    {
        StateIns ins;

        switch (config.stateEffects[i])
        {
            case StateEffect.DamageAbsorb:
                ins = new DamageAbsorb(owner, config, i, passive);
                break;
            case StateEffect.DamageRebound:
                ins = new DamageRebound(owner, config, i, passive);
                break;
            case StateEffect.DamageResist:
                ins = new DamageResist(owner, config, i, passive);
                break;
            case StateEffect.DamageStore:
                ins = new DamageStore(owner, config, i, passive);
                break;
            case StateEffect.DamageTransfer:
                ins = new DamageTransfer(owner, config, i, passive);
                break;
        }

        return null;
    }
}
