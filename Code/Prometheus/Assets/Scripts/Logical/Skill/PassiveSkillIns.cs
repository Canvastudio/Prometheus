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

        this.owner = owner;

        if (passive_config.stateType == StateType.Halo && passive_config.stateArg.f[0] > 0)
        {
            int range = Mathf.FloorToInt(passive_config.stateArg.f[0]);
            LiveItemSide side = 0;
            if (passive_config.stateArg.b[0])
            {
                if (owner.Side == LiveItemSide.SIDE0 )
                {
                    side = LiveItemSide.SIDE1;
                }
                else
                {
                    side = LiveItemSide.SIDE0;
                }
            }

            haloInfo = new HaloInfo(range, side, owner, this);
        }
   
        stateIns = new StateIns(state_config, owner, true);
    }

    public void Active()
    {
        stateIns.ActiveIns();
        owner.AddStateIns(stateIns);
        if (haloInfo != null) haloInfo.Active();
    }

    public void Deactive()
    {
        stateIns.DeactiveIns();
        owner.RemoveStateIns(stateIns);
        if (haloInfo != null) haloInfo.Deactive();
    }
}
