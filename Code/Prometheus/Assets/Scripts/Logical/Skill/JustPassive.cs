using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustPassive 
{
    private Dictionary<GameProperty, float> changes = new Dictionary<GameProperty, float>();

    public JustPassive(PassiveSkillsConfig config, int index, FightComponet fightComponet)
    {

    }

    public void Apply()
    {
        //if (changes.Count == 0)
        //{
        //    GameProperty property;

        //    if (passiveConfig != null)
        //    {
        //        var types = passiveConfig.passiveType.ToArray();

        //        if (types[index] == PassiveType.Just)
        //        {
        //            var value = FightComponet.CalculageRPN(
        //                passiveConfig.passiveSkillArgs[index].rpn.ToArray(0),
        //                fightComponet.ownerObject, null,
        //                out property);


        //            float origin_value = fightComponet.ownerObject.Property.GetFloatProperty(property);

        //            float change = value - origin_value;

        //            fightComponet.ownerObject.Property.SetFloatProperty(property, value);

        //            changes.Add(property, change);
        //        }
        //    }

        //}
        //else
        //{
        //    foreach (var change in changes)
        //    {
        //        float origin_value = fightComponet.ownerObject.Property.GetFloatProperty(change.Key);

        //        float value = origin_value + change.Value;

        //        fightComponet.ownerObject.Property.SetFloatProperty(change.Key, value);
        //    }
        //}
    }

    public void Remove()
    {
        //foreach (var change in changes)
        //{
        //    float origin_value = fightComponet.ownerObject.Property.GetFloatProperty(change.Key);

        //    float value = origin_value - change.Value;

        //    fightComponet.ownerObject.Property.SetFloatProperty(change.Key, value);
        //}
    }
}
