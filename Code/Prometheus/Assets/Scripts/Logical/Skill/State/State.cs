using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

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

[System.Serializable]
public class StateIns
{
    static int _id = 0;

    public int id;

    public StateEffectIns[] stateEffects;
    public StateConfig stateConfig;

    public ulong state_id;

    public bool passive;
    public bool active;
    public float exist_time;
    private bool _out_data = false;
    public float skillDamage = 0;
    public bool out_data
    {
        get { return _out_data; }
        set { if (value) OnOutData(); _out_data = value; }
    }

    public StateIns(StateConfig stateConfig, LiveItem owner, bool passive, LiveItem source = null, float _skillDamage = 0)
    {
        int effect_count = stateConfig.stateEffects.Count();
        skillDamage = _skillDamage;
        this.stateConfig = stateConfig;
        stateEffects = new StateEffectIns[effect_count];

        for (int i = 0; i < effect_count; ++i)
        {
            stateEffects[i] = StateEffectIns.GenerateStateEffects(stateConfig, i, owner, passive, source);
            stateEffects[i].skillDamage = _skillDamage;
        }

        id = ++_id;
        state_id = stateConfig.id;
    }

    public void ActiveIns()
    {
        if (!active)
        {
            active = true;

            foreach (var effect in stateEffects)
            {
                effect.Active();
            }

            if (!passive)
            {
                Messenger<float>.AddListener(SA.StageTimeCast, OnTimeChange);
            }
        }
    }

    public void DeactiveIns()
    {
        if (active)
        {
            active = false;

            if (stateEffects != null)
            {
                foreach (var effect in stateEffects)
                {
                    effect.Deactive();
                }
            }
            else
            {
                Debug.LogError("stateEffects为空: " + stateConfig.name);
            }

            if (!passive)
            {
                Messenger<float>.RemoveListener(SA.StageTimeCast, OnTimeChange);
            }
        }
    }
    
    private void OnOutData()
    {
        foreach (var effect in stateEffects)
        {
            effect.out_data = true;
        }

        DeactiveIns();
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

