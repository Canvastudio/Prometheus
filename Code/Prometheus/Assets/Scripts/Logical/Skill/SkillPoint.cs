﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillPointSkillType
{
    None,
    Active,
    Passive,
    Summon,
}

public class SkillPoint {

    public ulong id;
    public ulong[] skillIds;
    public int[] updateLimit;

    private ulong skillId;
    private int _count;
    public int last_count;
    private SkillPointSkillType _skillType;
    public SkillPointSkillType skillType
    {
        get
        {
            return _skillType;
        }
    }
    public int count
    {
        get { return _count; }
    }

    public SkillPoint(ulong _id)
    {
        id = _id;
        SkillPointsConfig config = ConfigDataBase.GetConfigDataById<SkillPointsConfig>(id);
        skillIds = config.skillIds.ToArray();
        updateLimit = config.characterActivate.ToArray((int)(StageCore.Instance.Player.typeId));
    }

    public void ChangeSkillPoint(int change_count)
    { 
        this._count = _count + change_count;

#if UNITY_EDITOR
        if (_count < 0) Debug.LogError("skill point 数量小于了0.");
#endif
    }

    public void GetSkillConfig(out ActiveSkillsConfig asc, out PassiveSkillsConfig psc, out SummonSkillsConfig ssc)
    {
        last_count = _count;

        asc = null;
        psc = null;
        ssc = null;

        for (int i = updateLimit.Length; i >= 0; --i)
        {
            if (updateLimit[i] <= _count)
            {
                skillId = skillIds[i];

                if (ConfigDataBase.ExistsId<ActiveSkillsConfig>(skillId))
                {
                    asc = ConfigDataBase.GetConfigDataById<ActiveSkillsConfig>(skillId);
                    _skillType = SkillPointSkillType.Active;
                }
                else if (ConfigDataBase.ExistsId<PassiveSkillsConfig>(skillId))
                {
                    psc = ConfigDataBase.GetConfigDataById<PassiveSkillsConfig>(skillId);
                    _skillType = SkillPointSkillType.Passive;
                }
                else if (ConfigDataBase.ExistsId<PassiveSkillsConfig>(skillId))
                {
                    ssc = ConfigDataBase.GetConfigDataById<SummonSkillsConfig>(skillId);
                    _skillType = SkillPointSkillType.Summon;
                }

                return;
            }
        }

        _skillType = SkillPointSkillType.None;
    }

    

}
