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

    //public float ohp, ospeed, omelee, olaser, ocartridge, oatk, omoto, ocapacity, oatkspeed, oreloadspeed, oshield, oarmor;
}
