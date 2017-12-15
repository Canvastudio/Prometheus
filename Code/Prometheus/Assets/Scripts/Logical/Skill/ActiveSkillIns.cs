using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActiveSkillIns {

    public int count = -1;

    public ulong skillId;
    public LiveItem owner;
    [SerializeField]
    protected bool active = false;
    public ActiveSkillsConfig config;
    public SkillPoint point;

    public ActiveSkillIns(ActiveSkillsConfig config, LiveItem owner, SkillPoint point, int count = -1)
    {
        this.owner = owner;
        this.config = config;
        this.point = point;
        this.skillId = config.id;
        this.count = count;
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

    public int UseSkill()
    {
        return --count;
    }
}
