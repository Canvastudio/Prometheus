using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : GameItemBase, IReactive {

    public SupplyConfig config;

    public void Reactive()
    {
        switch(config.supplyType)
        {
            case SupplyType.Box:
                Debug.Log("TODO: 开宝箱获得材料！");
                break;
            case SupplyType.Recover:
                Debug.Log(string.Format("TODO: 回复血量: {0}！", config.arg));
                StageCore.Instance.Player.AddHpPercent(float.Parse(config.arg));
                break;
        }

        GameObject.Destroy(this.gameObject);
    }
}
