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
    public StateType type = StateType.Normal;
    public int range;

    public PassiveSkillIns(ulong skill_id, SkillPoint _point, LiveItem owner)
    {
        skillId = skill_id;
        passiveConfig = ConfigDataBase.GetConfigDataById<PassiveSkillsConfig>(skill_id);
        stateConfig = ConfigDataBase.GetConfigDataById<StateConfig>(passiveConfig.bindState);
        point = _point;
        this.owner = owner;
        stateIns = new StateIns(stateConfig, owner, true);
        owner.state.AddStateIns(stateIns);
        type = passiveConfig.stateType;

        if (type == StateType.Halo)
        {
            range = Mathf.FloorToInt(passiveConfig.stateArg.f[0]);
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
    }

    public void Active()
    {
        stateIns.ActiveIns();
  
        if (haloInfo != null) haloInfo.Active();
    }

    public void Deactive()
    {
        stateIns.DeactiveIns();

        //if (haloInfo != null) haloInfo.Deactive();
    }

    public void Remove()
    {
        owner.state.RemoveStateIns(stateIns);
        //if (haloInfo != null) haloInfo.Deactive();
    }
}
