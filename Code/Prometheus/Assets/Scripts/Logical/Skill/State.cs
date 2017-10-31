using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum StateEffectType
{
    DamageRelate,
    Invalid,
}

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

    public StateEffectIns(LiveItem owner, StateConfig config, int index, bool passive)
    {
        this.owner = owner;
        this.stateConfig = config;
        this.index = index;
        this.passive = passive;
    }

    protected virtual IEnumerator Apply(Damage damageInfo) { return null; }

    public virtual void OnOutData()
    {
        stateConfig = null;

        owner.RemoveDefendState(this);

        owner = null;
    }

    public bool Equals(StateEffectIns other)
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
                case StateEffectType.DamageRelate:
                    ie = Apply(param as Damage);
                    break;
            }

            if (ie != null)
            {
                yield return ie;
            }
        }
    }

    public static void GenerateStateEffects(StateConfig config, LiveItem owner, bool passive, out StateEffectIns[] effectIns)
    {
        var effects = config.stateEffects.ToArray();
        effectIns = new StateEffectIns[effects.Length];

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
