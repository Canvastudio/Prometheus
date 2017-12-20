using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPoint {

    public ulong id;
    public ulong[] skillIds;
    public int[] updateLimit;

    SkillPointsConfig config;

    public ulong skillId;
    private int _count;
    public int last_count;

    public int activeIndex = -1;



    public int count
    {
        get { return _count; }
    }

    public SkillPoint(ulong _id)
    {
        id = _id;
        config = ConfigDataBase.GetConfigDataById<SkillPointsConfig>(id);
        skillIds = config.skillIds.ToArray();
        updateLimit = config.characterActivate.ToArray((int)(StageCore.Instance.Player.playerId));
    }

    public void ChangeSkillPoint(int change_count)
    { 
        this._count = _count + change_count;
    }

    public ulong GetNewSkillId()
    {
        last_count = _count;

        for (int i = updateLimit.Length - 1; i >= 0; --i)
        {
            if (updateLimit[i] <= _count)
            {
                skillId = skillIds[i];
                activeIndex = i;
                 return skillId;
            }
        }

        skillId = 0;
        return 0;
    }

    

}
