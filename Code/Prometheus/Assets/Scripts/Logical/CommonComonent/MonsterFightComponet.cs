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
    public List<MonsterActiveSkillIns> monsterActiveInsList = new List<MonsterActiveSkillIns>(2);

    public override void Clean()
    {
        DeactiveSkill();
        monsterActiveInsList.Clear();

        base.Clean();
    }

    protected override void AddActiveIns(ulong id, SkillPoint point)
    {
        ActiveSkillsConfig aconfig = ConfigDataBase.GetConfigDataById<ActiveSkillsConfig>(id);
        if (aconfig == null)
        {
            Debug.LogError("给怪物添加了一个不存在的技能, id: " + id);
            return;
        }
        monsterActiveInsList.Add(new MonsterActiveSkillIns(aconfig, ownerObject, point));
    }

    public void ReorderSkill()
    {
        monsterActiveInsList.Sort(comparison);
    }

    public override void RemoveSkill(ulong id)
    {
        base.RemoveSkill(id);

        switch (IdToSkillType(id))
        {
            case SkillType.Active:
                for (int i = 0; i < monsterActiveInsList.Count; ++i)
                {
                    if (monsterActiveInsList[i].config.id == id)
                    {
                        monsterActiveInsList[i].Deactive();
                        monsterActiveInsList.RemoveAt(i);
                    }
                }
                break;
        }
    }

    public override void ActiveSkill()
    {
        if (!skillActive)
        {
            foreach (var ins in monsterActiveInsList)
            {
                ins.Active();
            }

            Messenger<float>.AddListener(SA.StageTimeCast, OnTimeCast);
        }

        ReorderSkill();

        base.ActiveSkill();
    }

    public override void DeactiveSkill()
    {
        if (skillActive)
        {
            foreach (var ins in monsterActiveInsList)
            {
                ins.Deactive();
            }

            Messenger<float>.RemoveListener(SA.StageTimeCast, OnTimeCast);
        }

        base.DeactiveSkill();
    }

    bool releaseSkill = false;

    private void OnTimeCast(float time)
    {
        bool one_ready = false;

        foreach (var ins in monsterActiveInsList)
        {
            if(!one_ready && ins.OnTimeCast(time))
            {
                StartCoroutine(DoSkill(ins));
            }
        }

        if (one_ready)
        {
            ReorderSkill();
        }
    }

    private IEnumerator DoSkill(MonsterActiveSkillIns ins)
    {
        if (!releaseSkill)
        {
            releaseSkill = true;
            yield return DoActiveSkill(ins);
            releaseSkill = false;

            if (activeSkillSuccess)
            {
                activeSkillSuccess = false;
                ins.time = StageCore.Instance.totalTime;
                ins.cooldown = ins.config.coolDown;
            }
        }


    }
}
