using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveSkillIns  {

    public StateIns[] stateIns;
    public HaloInfo haloInfo;
    public LiveItem owner;

    public PassiveSkillIns(ulong skill_id)
    {
        var passive_config = ConfigDataBase.GetConfigDataById<PassiveSkillsConfig>(skill_id);

        if (passive_config.stateType == StateType.Halo)
        {
            haloInfo = new HaloInfo(passive_config.stateArg);
        }

        var state_config = ConfigDataBase.GetConfigDataById<StateConfig>(passive_config.bindState);

        int state_count = state_config.stateEffects.Count();
        stateIns = new StateIns[state_count];

        for (int i = 0; i < state_config.stateEffects.Count(); ++i)
        {
            stateIns[i] = StateIns.GenerateStateEffects(state_config, i, owner, true);
        }
    }
}
