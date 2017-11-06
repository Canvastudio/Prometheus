using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFightComponet : FightComponet {

    List<MonsterActiveSkillIns> activeInsList = new List<MonsterActiveSkillIns>(2);

    public override void AddSkill(ulong id)
    {
        base.AddSkill(id);

        switch (IdToSkillType(id))
        {
            case SkillType.Active:
                MonsterActiveSkillIns ins = new MonsterActiveSkillIns(ConfigDataBase.GetConfigDataById<ActiveSkillsConfig>(id), ownerObject);
                activeInsList.Add(ins);
                break;
        }
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

    public override void Active()
    {
        base.Active();

        if (!active)
        {
            foreach (var ins in activeInsList)
            {
                ins.Active();
                Messenger<float>.AddListener(SA.StageTimeCast, OnTimeCast);
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
            }
        }
    }

    public override void Deactive()
    {
        if (active)
        {
            foreach (var ins in activeInsList)
            {
                ins.Deactive();
                Messenger<float>.RemoveListener(SA.StageTimeCast, OnTimeCast);
            }
        }
    }

}
