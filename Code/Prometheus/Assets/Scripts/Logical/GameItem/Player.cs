﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : LiveItem {

    public PlayerInitConfig config;

    public Player SetPlayerProperty(float motorized, float capacity, float atkSpeed, float reloadSpeed)
    {
        property.SetFloatProperty(GameProperty.motorized, motorized)

            .SetFloatProperty(GameProperty.atkSpeed, atkSpeed)
            .SetFloatProperty(GameProperty.reloadSpeed, reloadSpeed);

        return this;
    }

    public override IEnumerator MeleeAttackTarget<T>(T target)
    {
        var e = base.MeleeAttackTarget(target);

        var config = ConfigDataBase.GetConfigDataById<GlobalParameterConfig>(1);
        var atk_Speed = property[GameProperty.atkSpeed];
        var timeSpend = (1 - ((atk_Speed + 100) / (atk_Speed + 101))) * 15; 

        
        StageCore.Instance.AddTurnTimeAndMoveDown(timeSpend);


        return e;
    }

}
