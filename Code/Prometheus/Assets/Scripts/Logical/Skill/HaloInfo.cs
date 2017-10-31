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
    public StateEffectIns[] effectIns;

    public HaloInfo(PassiveSkillsConfig config, LiveItem owner)
    {
        this.config = config;
        StateConfig stateConfig = ConfigDataBase.GetConfigDataById<StateConfig>(config.passiveSkillArgs[0].u[0]);
        StateEffectIns.GenerateStateEffects(stateConfig, owner, true, out effectIns);
    }
}
