using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiveBaseProperty : PropertyData
{
    public LiveBaseProperty InitBaseProperty(
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
        SetFloatProperty("nhp", 0f);
        return this;
    }
}
