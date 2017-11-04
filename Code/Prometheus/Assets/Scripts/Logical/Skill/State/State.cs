using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum StateEffectType
{
    OnTakenDamage,
    OnGenerateDamage,
    OnDeadth,
    PropertyChange,
    JustPropertyChange,
    RangeSkillCost,
    /// <summary>
    /// 自然消失的时候起效果
    /// </summary>
    Countdown, 
    /// <summary>
    /// 杀死目标的时候有额外效果
    /// </summary>
    KillTarget,
    Invalid,
    /// <summary>
    /// 无法被选择
    /// </summary>
    SelectImmune,
    /// <summary>
    /// 如果有非last状态的怪物存在，那么就不受伤害
    /// </summary>
    Last,
}


public class StateIns
{
    static int _id = 0;

    public int id;
    public StateEffectIns[] stateEffects;
    public StateConfig stateConfig;
    public bool passive;
    public bool active;
    public float exist_time;
    private bool _out_data = false;
    public bool out_data
    {
        get { return _out_data; }
        set { if (value) OnOutData(); out_data = value; }
    }

    public StateIns(StateConfig stateConfig, LiveItem owner, bool passive)
    {
        int effect_count = stateConfig.stateEffects.Count();

        stateEffects = new StateEffectIns[effect_count];

        for (int i = 0; i < effect_count; ++i)
        {
            stateEffects[i] = StateEffectIns.GenerateStateEffects(stateConfig, i, owner, passive);
        }

        id = ++_id;
    }

    public void ActiveIns()
    {
        foreach(var effect in stateEffects)
        {
            effect.Active();
        }

        if (!passive)
        {
            Messenger<float>.AddListener(SA.StageTimeCast, OnTimeChange);
        }
    }

    public void DeactiveIns()
    {
        foreach (var effect in stateEffects)
        {
            effect.Deactive();
        }


        if (!passive)
        {
            Messenger<float>.RemoveListener(SA.StageTimeCast, OnTimeChange);
        }
    }
    
    private void OnOutData()
    {
        foreach (var effect in stateEffects)
        {
            effect.out_data = true;
        }
    }

    public void Silent(bool silent)
    {
        if (!passive) return;

        if (silent)
        {
            foreach(var effect in stateEffects)
            {
                effect.Deactive();
            }
        }
        else
        {
            foreach (var effect in stateEffects)
            {
                effect.Active();
            }
        }
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

    protected virtual void OnTimeChange(float time)
    {
        exist_time += time;

        if (exist_time >= stateConfig.
            time)
        {
            out_data = true;
        }
        else
        {
            foreach (var effect in stateEffects)
            {
                effect.OnTimeChange(time);
            }
        }
    }

    public override bool Equals(object obj)
    {
        return stateConfig.id == (obj as StateIns).stateConfig.id;
    }
}

/// <summary>
/// 结算类的状态的具体效果，比如伤害结算的减少伤害的， 攻击结算的造成暴击的
/// </summary>
public abstract class StateEffectIns : IEquatable<StateEffectIns>
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

    public StateEffectIns(LiveItem owner, StateConfig config, int index, bool passive)
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
    }

    public virtual void Deactive()
    {
        active = false;
    }

    public virtual void OnTimeChange(float time)
    {

    }

    protected abstract void Apply(object param);

    public virtual void OnOutData()
    {
        stateConfig = null;
        owner = null;
    }

    public bool Equals(StateEffectIns other)
    {
        return (stateConfig.id == other.stateConfig.id)
            && (index == other.index);
    }

    public void ApplyState(object param)
    {
        if (!owner.Silent || out_data || active == false)
        {
            //if (stateType == StateEffectType.OnGenerateDamage
            //    || stateType == StateEffectType.OnTakenDamage)
            //{
            //    Apply(param as Damage);
            //}
            //else if (stateType == StateEffectType.RangeSkillCost)
            //{
            //    Apply(param as RangeSkillCost);
            //}

            Apply(param);
        }
    }

    public static StateEffectIns GenerateStateEffects(StateConfig config, int i, LiveItem owner, bool passive)
    {
        StateEffectIns ins;

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
