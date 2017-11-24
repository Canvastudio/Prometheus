using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 结算类的状态的具体效果，比如伤害结算的减少伤害的， 攻击结算的造成暴击的
/// </summary>
[System.Serializable]
public abstract class StateEffectIns : IEquatable<StateEffectIns>
{
    public StateEffectType stateType = StateEffectType.Invalid;
    public StateConfig stateConfig;

    public LiveItem source;
    public int index;
    protected bool passive;
    public LiveItem owner;
    public float skillDamage;
    private bool _out_data = false;
    public bool out_data
    {
        get { return _out_data; }
        set { if (value) OnOutData(); _out_data = value; }
    }

    public bool active = false;

    /// <summary>
    /// 存在了多久
    /// </summary>
    public float exist_time = 0;

    public StateEffectIns(LiveItem owner, StateConfig config, int index, bool passive, LiveItem source)
    {
        this.owner = owner;
        this.stateConfig = config;
        this.index = index;
        this.passive = passive;
        this.source = source;
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

    public virtual void Remove()
    {

    }

    public virtual void OnTimeChange(float time)
    {

    }

    protected abstract void Apply(object param);

    public virtual void OnOutData()
    {
        stateConfig = null;

        if (active)
        {
            Deactive();
        }

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

    public static StateEffectIns GenerateStateEffects(StateConfig config, int i, LiveItem owner, bool passive, LiveItem source)
    {
        StateEffectIns ins = null;
        Type t = null;
        //switch (config.stateEffects[i])
        //{
        //    case StateEffect.DamageAbsorb:
        //        ins = new DamageAbsorb(owner, config, i, passive);
        //        break;
        //    case StateEffect.DamageRebound:
        //        ins = new DamageRebound(owner, config, i, passive);
        //        break;
        //    case StateEffect.DamageResist:
        //        ins = new DamageResist(owner, config, i, passive);
        //        break;
        //    case StateEffect.DamageStore:
        //        ins = new DamageStore(owner, config, i, passive);
        //        break;
        //    case StateEffect.DamageTransfer:
        //        ins = new DamageTransfer(owner, config, i, passive);

        //        break;
        //    default:
        //        Debug.LogError("未完成的StateEffect: " + config.stateEffects[i].ToString());
        //        break;
        //}


        try
        {
            t = Type.GetType(config.stateEffects[i].ToString());
            ins = (StateEffectIns)Activator.CreateInstance(t, owner, config, i, passive, source);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Activator Exception: " + e.Message);
            Debug.LogError("State: " + config.stateEffects[i].ToString());
        }

        return ins;
    }
}
