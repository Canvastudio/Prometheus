using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiveBasePropertys : PropertyData {

    public LiveBasePropertys InitMonsterProperty(
        float hp,
        float atk,
        float shield,
        float armor)
    {
        SetFloatProperty(GameProperty.mhp, hp);
        SetFloatProperty(GameProperty.attack, atk);
        SetFloatProperty(GameProperty.shield, shield);
        SetFloatProperty(GameProperty.guard, armor);
        SetFloatProperty(GameProperty.nhp, hp);
        SetFloatProperty(GameProperty.nshield, shield);
        return this;
    }

    public LiveBasePropertys InitPlayerProperty(
        float hp,
        float fatk,
        float atk,
        float laser,
        float shield,
        float armor)
    {
        SetFloatProperty(GameProperty.mhp, hp);
        SetFloatProperty(GameProperty.firstAtt, fatk);
        SetFloatProperty(GameProperty.attack, atk);
        SetFloatProperty(GameProperty.shield, shield);
        SetFloatProperty(GameProperty.guard, armor);
        SetFloatProperty(GameProperty.nhp, hp);
        SetFloatProperty(GameProperty.nshield, shield);
        SetFloatProperty(GameProperty.laser, laser);

        return this;
    }

}
