using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterProperty : PropertyData
{
    public MonsterProperty InitBaseProperty(
    float hp,
    float speed,
    float melee,
    float laser,
    float cartridge)
    {
        SetFloatProperty("mhp", hp);
        SetFloatProperty("speed", speed);
        SetFloatProperty("melee", melee);
        SetFloatProperty("laser", laser);
        SetFloatProperty("cartridge", cartridge);

        return this;
    }
}
