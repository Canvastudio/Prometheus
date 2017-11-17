using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFightComponet : FightComponet {

    static System.Comparison<MonsterActiveSkillIns> comparison = (MonsterActiveSkillIns a, MonsterActiveSkillIns b) =>
    {
        if (a.cooldown > b.cooldown)
        {
            return 1;
        }
        else if (a.cooldown < b.cooldown)
        {
            return -1;
        }
        else if (a.time < b.time)
        {
            return 1;
        }
        else if (a.time > b.time)
        {
            return -1;
        }
        else return 0;
    };

    [SerializeField]
    public new List<MonsterActiveSkillIns> activeInsList = new List<MonsterActiveSkillIns>(2);

    protected new void AddActiveIns(ulong id, SkillPoint point)
    {
        ActiveSkillsConfig aconfig = ConfigDataBase.GetConfigDataById<ActiveSkillsConfig>(id);
        activeInsList.Add(new MonsterActiveSkillIns(aconfig, ownerObject, point));
    }

    public void ReorderSkill()
    {
        activeInsList.Sort(comparison);
    }

    public override void RemoveSkill(ulong id)
    {
        base.RemoveSkill(id);

        switch (IdToSkillType(id))
        {
            case SkillType.Active:
                for (int i = 0; i < activeInsList.Count; ++i)
                {
                    if (activeInsList[i].config.id == id)
                    {
                        activeInsList[i].Deactive();
                        activeInsList.RemoveAt(i);
                    }
                }
                break;
        }
    }

    public override void ActiveSkill()
    {
        base.ActivePassive();

        if (!activePassive)
        {
            foreach (var ins in activeInsList)
            {
                ins.Active();
                Messenger<float>.AddListener(SA.StageTimeCast, OnTimeCast);
            }
        }

        ReorderSkill();
    }

    public override void DeactiveSkill()
    {
        if (activePassive)
        {
            foreach (var ins in activeInsList)
            {
                ins.Deactive();
                Messenger<float>.RemoveListener(SA.StageTimeCast, OnTimeCast);
            }
        }
    }

    private void OnTimeCast(float time)
    {
        bool one_ready = false;

        foreach (var ins in activeInsList)
        {
            if(!one_ready && ins.OnTimeCast(time))
            {
                StartCoroutine(DoActiveSkill(ins.config));
                ins.time = StageCore.Instance.totalTime;
            }
        }

        if (one_ready)
        {
            ReorderSkill();
        }
    }



}
