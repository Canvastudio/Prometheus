using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveSkillIns  {

    public StateIns stateIns;
    public HaloInfo haloInfo;
    public LiveItem owner;
    public StateConfig stateConfig;

    public PassiveSkillIns(ulong skill_id, LiveItem owner)
    {
        var passive_config = ConfigDataBase.GetConfigDataById<PassiveSkillsConfig>(skill_id);

        var state_config = ConfigDataBase.GetConfigDataById<StateConfig>(passive_config.bindState);

        stateIns = new StateIns(state_config, owner, true);

        if (passive_config.stateType == StateType.Halo)
        {
            int range = Mathf.FloorToInt(passive_config.stateArg.f[0]);
            int side = 0;
            if (passive_config.stateArg.b[0])
            {
                if (owner.side == 0)
                {
                    side = 1;
                }
                else
                {
                    side = 0;
                }
            }

            haloInfo = new HaloInfo(range, side, owner, this);
        }
    }
}
