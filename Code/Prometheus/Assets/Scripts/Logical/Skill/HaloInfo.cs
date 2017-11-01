using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 光环信息
/// </summary>
public class HaloInfo  {

    public PassiveSkillsConfig config;
    public int range = 0;
    public bool forEmemy = false;
    public DamageState[] effectIns;

    public HaloInfo(PassiveSkillsConfig config, LiveItem owner)
    {

    }
}
