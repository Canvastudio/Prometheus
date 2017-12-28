using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiveBasePropertys : PropertyData {

    public LiveBasePropertys InitBaseProperty(
        float hp,
        //float speed,
        //float melee,
        //float laser,
        //float cartridge,
        //float atk
        float shield,
        float armor)
    {
        SetFloatProperty(GameProperty.mhp, hp);
        SetFloatProperty(GameProperty.shield, shield);
        SetFloatProperty(GameProperty.guard, armor);
        //SetFloatProperty(GameProperty.speed, speed);
        //SetFloatProperty(GameProperty.melee, melee);
        //SetFloatProperty(GameProperty.laser, laser);
        //SetFloatProperty(GameProperty.cartridge, cartridge);
        //SetFloatProperty(GameProperty.nhp, hp);
        //SetFloatProperty(GameProperty.attack, atk);

        //ohp = hp;
        //ospeed = speed;
        //omelee = melee;
        //olaser = laser;
        //ocartridge = cartridge;
        //oatk = atk;

        return this;
    }

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
  
        return this;
    }

    public LiveBasePropertys InitPlayerProperty(
        float hp,
        float fatk,
        float melee,
        float laser,
        float shield,
        float armor)
    {
        SetFloatProperty(GameProperty.mhp, hp);
        SetFloatProperty(GameProperty.firstAtt, fatk);
        SetFloatProperty(GameProperty.melee, melee);
        SetFloatProperty(GameProperty.shield, shield);
        SetFloatProperty(GameProperty.guard, armor);

        return this;
    }

}
