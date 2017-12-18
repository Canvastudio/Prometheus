using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiveBasePropertys : PropertyData {

    public LiveBasePropertys InitBaseProperty(
        float hp,
        float speed,
        float melee,
        float laser,
        float cartridge,
        float atk)
    {
        SetFloatProperty(GameProperty.mhp, hp);
        SetFloatProperty(GameProperty.speed, speed);
        SetFloatProperty(GameProperty.melee, melee);
        SetFloatProperty(GameProperty.laser, laser);
        SetFloatProperty(GameProperty.cartridge, cartridge);
        SetFloatProperty(GameProperty.nhp, hp);
        SetFloatProperty(GameProperty.attack, atk);

        return this;
    }
}
