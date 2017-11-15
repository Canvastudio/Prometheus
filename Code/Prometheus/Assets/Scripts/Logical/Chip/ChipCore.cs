using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipCore : SingleObject<ChipCore> {

    /// <summary>
    /// 芯片盘升级次数
    /// </summary>
    public int chipBoardUpdate = 20;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="chip1"></param>
    /// <param name="chip2"></param>
    /// <returns></returns>
    public ChipConfig ChipMerge(ChipInventory chip1, ChipInventory chip2, out int cost)
    {
        if (chip1.config.id != chip2.config.id)
        {
            throw new System.ArgumentException("id不相同的的芯片无法合成!");
        }
        else
        {
            ulong nid = chip1.config.upgradeId;
            ChipConfig nconfig = ConfigDataBase.GetConfigDataById<ChipConfig>(nid);
            cost = Mathf.FloorToInt((chip1.cost + chip2.cost) / 2f + Mathf.Abs((chip1.cost - chip2.cost)) / 4f);
            return nconfig;
        }
    }
}
