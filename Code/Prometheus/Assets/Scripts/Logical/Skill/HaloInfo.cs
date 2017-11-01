using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 光环信息
/// </summary>
public class HaloInfo  {

    /// <summary>
    /// 影响到的目标,包括自己
    /// </summary>
    List<LiveItem> effect_target = new List<LiveItem>(5);

    float range = 0;
    bool enemy = false;

    public HaloInfo(SkillArg arg)
    {

    }

}
