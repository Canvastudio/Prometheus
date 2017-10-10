using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : LiveItem {

    public PlayerInitConfig config;

    public PropertyData playerProperty = new PropertyData();

    public override IEnumerator AttackByOther<T>(T other)
    {
        return null;
    }

    public override IEnumerator AttackTarget<T>(T target)
    {
        return null;
    }

    public Player SetPlayerProperty(float motorized, float capacity, float atkSpeed, float reloadSpeed)
    {
        playerProperty.SetFloatProperty("motorized", motorized)
            .SetFloatProperty("capacity", capacity)
            .SetFloatProperty("atkSpeed", atkSpeed)
            .SetFloatProperty("reloadSpeed", reloadSpeed);

        return this;
    }

}
