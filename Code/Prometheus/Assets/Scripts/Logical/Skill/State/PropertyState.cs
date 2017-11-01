using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropertyState : StateIns {

    protected Dictionary<GameProperty, float> changes = new Dictionary<GameProperty, float>();

    public PropertyState(LiveItem owner, StateConfig config, int index, bool passive) : base(owner, config, index, passive)
    {
        stateType = StateEffectType.PropertyChange;
    }

    public void Remove()
    {
        foreach (var change in changes)
        {
            float origin_value = owner.Property.GetFloatProperty(change.Key);

            float value = origin_value - change.Value;

            owner.Property.SetFloatProperty(change.Key, value);
        }
    }

    protected override void Apply(object param)
    {
        LiveItem target = param as LiveItem;

        GameProperty property;

        if (stateConfig != null)
        {
            var value = FightComponet.CalculageRPN(
                stateConfig.stateArgs[index].rpn.ToArray(0),
                owner, target,
                out property);

            float origin_value = owner.Property.GetFloatProperty(property);

            float change = value - origin_value;

            owner.Property.SetFloatProperty(property, value);

            changes.Add(property, change);
        }
    }
}
