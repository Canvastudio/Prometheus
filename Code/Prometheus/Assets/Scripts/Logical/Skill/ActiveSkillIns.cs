using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActiveSkillIns {

    public LiveItem owner;
    protected bool active = false;
    public ActiveSkillsConfig config;
    public SkillPoint point;

    public ActiveSkillIns(ActiveSkillsConfig config, LiveItem owner, SkillPoint point)
    {
        this.owner = owner;
        this.config = config;
        this.point = point;
    }

    public virtual void Active()
    {
        if (!active)
        {
            active = true;
        }
    }

    public virtual void Deactive()
    {
        if (active)
        {
            active = false;
        }
    }
}
