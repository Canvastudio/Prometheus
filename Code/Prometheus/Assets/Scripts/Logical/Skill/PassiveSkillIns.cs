using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PassiveSkillIns  {

    public ulong skillId;
    public StateIns stateIns;
    public HaloInfo haloInfo;
    public LiveItem owner;
    public StateConfig stateConfig;
    public PassiveSkillsConfig passiveConfig;
    public SkillPoint point;

    public PassiveSkillIns(ulong skill_id, SkillPoint _point, LiveItem owner)
    {
        skillId = skill_id;
        passiveConfig = ConfigDataBase.GetConfigDataById<PassiveSkillsConfig>(skill_id);
        stateConfig = ConfigDataBase.GetConfigDataById<StateConfig>(passiveConfig.bindState);
        point = _point;
        this.owner = owner;
       

        if (passiveConfig.stateType == StateType.Halo && passiveConfig.stateArg.f[0] > 0)
        {
            int range = Mathf.FloorToInt(passiveConfig.stateArg.f[0]);
            LiveItemSide side = 0;
            if (passiveConfig.stateArg.b[0])
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
   
        stateIns = new StateIns(stateConfig, owner, true);
    }

    public void Active()
    {
        //stateIns.ActiveIns();
        owner.AddStateIns(stateIns);
        if (haloInfo != null) haloInfo.Active();
    }

    public void Deactive()
    {
        //stateIns.DeactiveIns();
        owner.RemoveStateIns(stateIns);
        if (haloInfo != null) haloInfo.Deactive();
    }
}
