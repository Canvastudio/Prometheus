using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterActiveSkillIns {

    public float cooldown;

    /// <summary>
    /// 上次释放的时间
    /// </summary>
    public float time;

    public LiveItem owner;
    private bool active = false;
    public ActiveSkillsConfig config;

    public MonsterActiveSkillIns(ActiveSkillsConfig config, LiveItem owner)
    {
        cooldown = config.coolDown;
        this.owner = owner;
        this.config = config;
    }

    public void Active()
    {
        if (!active)
        {
            active = true;
        }
    }

    public void Deactive()
    {
        if (active)
        {
            active = false;
        }
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
            cooldown -= time;

            if (cooldown <= 0)
            {
                cooldown = config.coolDown;
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
