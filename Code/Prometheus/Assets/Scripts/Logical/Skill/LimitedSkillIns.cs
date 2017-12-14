using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitedSkillIns {

    public ulong skillId;
    public int count;

    public LimitedSkillIns(ulong id, int count)
    {
        skillId = id;
        this.count = count;
    }

    public bool Use()
    {
        --count;

        if (count == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
