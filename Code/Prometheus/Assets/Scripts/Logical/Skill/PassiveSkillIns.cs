using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PassiveSkillIns
{

    static int _id = int.MinValue;

    public int id;

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
        id = _id++;

        skillId = skill_id;
        passiveConfig = ConfigDataBase.GetConfigDataById<PassiveSkillsConfig>(skill_id);
        stateConfig = ConfigDataBase.GetConfigDataById<StateConfig>(passiveConfig.bindState);
        point = _point;
        this.owner = owner;

        type = passiveConfig.stateType;

        if (type == StateType.Halo)
        {
            range = Mathf.FloorToInt(passiveConfig.stateArg.f[0]);

            LiveItemSide side = 0;

            if (passiveConfig.stateArg.b[0])
            {
                if (owner.Side == LiveItemSide.SIDE0)
                {
                    side = LiveItemSide.SIDE1;
                }
                else
                {
                    side = LiveItemSide.SIDE0;
                }
            }

            haloInfo = new HaloInfo(range, side, owner, this);

            //owner.state.AddHalo(haloInfo);
        }
        else
        {
            stateIns = new StateIns(stateConfig, owner, this, owner);
            owner.state.AddStateIns(stateIns);
        }
    }

    public void Active()
    {
        if (type == StateType.Normal)
        {
            stateIns.ActiveIns();
        }
        else
        {
            if (type == StateType.Halo) haloInfo.Active();
        }
    }

    public void Deactive()
    {
        if (type == StateType.Normal)
        {
            stateIns.DeactiveIns();
        }
        else
        {
            if (type == StateType.Halo) haloInfo.Deactive();
        }
    }

    public void Remove()
    {
        if (type == StateType.Normal)
        {
            owner.state.RemoveStateIns(stateIns);
        }
        else
        {
            if (type == StateType.Halo) haloInfo.Remove();
        }
    }
}
