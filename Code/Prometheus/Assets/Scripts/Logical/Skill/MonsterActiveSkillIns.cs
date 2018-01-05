using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterActiveSkillIns : ActiveSkillIns
{
    public float cooldown;

    /// <summary>
    /// 上次释放的时间
    /// </summary>
    public float time;

    public MonsterActiveSkillIns(ActiveSkillsConfig config, LiveItem owner, SkillPoint point) :base (config, owner, point)
    {
        cooldown = config.coolDown;
        this.owner = owner;
        this.config = config;
    }

    /// <summary>
    /// 时间流逝，如果冷却到0，就会释放
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public bool OnTimeCast(float time)
    {

        if (active)
        {
            if (cooldown >= 0)
            {
                cooldown -= time;
            }

            if (cooldown <= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}
