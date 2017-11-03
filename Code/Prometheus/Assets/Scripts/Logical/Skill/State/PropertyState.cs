using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropertyState : StateIns {

    protected Dictionary<GameProperty, float> changes = new Dictionary<GameProperty, float>();

    LiveItem source;
    /// <summary>
    /// Property因为有可能是别人给的伤害，那么需要确定伤害来源，所以需要提供一个source，表示谁提供了找个Property 状态
    /// </summary>
    /// <param name="source"></param>
    /// <param name="owner"></param>
    /// <param name="config"></param>
    /// <param name="index"></param>
    /// <param name="passive"></param>
    public PropertyState(LiveItem source, LiveItem owner, StateConfig config, int index, bool passive) : base(owner, config, index, passive)
    {
        stateType = StateEffectType.PropertyChange;
    }

    public void Remove()
    {
        foreach (var change in changes)
        {
            if (change.Key != GameProperty.nhp)
            {
                float origin_value = owner.Property.GetFloatProperty(change.Key);

                float value = origin_value - change.Value;

                owner.Property.SetFloatProperty(change.Key, value);
            }
        }
    }

    protected override void Apply(object param)
    {
        GameProperty property;

        if (stateConfig != null)
        {
            var value = FightComponet.CalculageRPN(
                stateConfig.stateArgs[index].rpn.ToArray(0),
                source, owner,
                out property);

            float origin_value = owner.Property.GetFloatProperty(property);
            float change = value - origin_value;


            if (property != GameProperty.nhp)
            {
                owner.Property.SetFloatProperty(property, value);
                changes.Add(property, change);
            }
            else
            {
                //如果修改属性是修改当前血量，那么就判定为一次物理伤害
                if (change < 0)
                {
                    owner.MeleeAttackByOther(owner, new Damage(-change, source, owner, DamageType.Physical));
                }
                else
                {
                    owner.Property.SetFloatProperty(property, value);
                }
            }
        }
    }
}
