using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum StateEffectType
{
    OnTakenDamage,
    OnGenerateDamage,
    PropertyChange,
    JustPropertyChange,
    /// <summary>
    /// 自然消失的时候起效果
    /// </summary>
    Countdown, 
    /// <summary>
    /// 杀死目标的时候有额外效果
    /// </summary>
    KillTarget,
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

    public bool active = false;

    /// <summary>
    /// 存在了多久
    /// </summary>
    public float exist_time = 0;

    public StateIns(LiveItem owner, StateConfig config, int index, bool passive)
    {
        this.owner = owner;
        this.stateConfig = config;
        this.index = index;
        this.passive = passive;
    }

    /// <summary>
    /// 状态被生成的时候不是激活状态，所以需要等到激活才能生效
    /// </summary>
    public virtual void Active()
    {
        active = true;

        if (!passive)
        {
            Messenger<float>.AddListener(SA.StageTimeCast, OnTimeChange);
        }
    }

    public virtual void Deactive()
    {
        active = false;

        if (!passive)
        {
            Messenger<float>.RemoveListener(SA.StageTimeCast, OnTimeChange);
        }
    }

    ~StateIns()
    {
        if (!passive)
        {
            Messenger<float>.RemoveListener(SA.StageTimeCast, OnTimeChange);
        }
    }

    protected virtual void OnTimeChange(float time)
    {
        exist_time += time;

        if (exist_time >= stateConfig.
            time)
        {
            out_data = true;
        }
    }

    protected abstract void Apply(object param);

    public virtual void OnOutData()
    {
        stateConfig = null;

        owner.RemoveStateIns(this);

        owner = null;
    }

    public bool Equals(StateIns other)
    {
        return (stateConfig.id == other.stateConfig.id)
            && (index == other.index);
    }

    public void ApplyState(object param)
    {
        if (!owner.Silent || out_data || active == false)
        {
            Apply(param as Damage);
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
