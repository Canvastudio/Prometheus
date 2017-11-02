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
        //if (changes.count == 0)
        //{
        //    gameproperty property;

        //    if (passiveconfig != null)
        //    {
        //        var types = passiveconfig.passivetype.toarray();

        //        if (types[index] == passivetype.just)
        //        {
        //            var value = fightcomponet.calculagerpn(
        //                passiveconfig.passiveskillargs[index].rpn.toarray(0),
        //                fightcomponet.ownerobject, null,
        //                out property);


        //            float origin_value = fightcomponet.ownerobject.property.getfloatproperty(property);

        //            float change = value - origin_value;

        //            fightcomponet.ownerobject.property.setfloatproperty(property, value);

        //            changes.add(property, change);
        //        }
        //    }

        //}
        //else
        //{
        //    foreach (var change in changes)
        //    {
        //        float origin_value = fightcomponet.ownerobject.property.getfloatproperty(change.key);

        //        float value = origin_value + change.value;

        //        fightcomponet.ownerobject.property.setfloatproperty(change.key, value);
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
