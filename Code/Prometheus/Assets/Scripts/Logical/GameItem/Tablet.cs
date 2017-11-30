using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tablet : GameItemBase {

    public TotemConfig config;

    private delegate bool BoolReturn();

    private BoolReturn CheckFunc;

    private List<Monster> m;

    [SerializeField]
    private float round = 0;

#if UNITY_EDITOR
    [SerializeField]
    TotemType type;
#endif

    /// <summary>
    /// 起效的回合数
    /// </summary>
    [SerializeField]
    private float activeRound = int.MaxValue;

    /// <summary>
    /// 效果整形参数1
    /// </summary>
    int int_arg1 = 0;

    [SerializeField]
    Monster lastDeadMonster;

    public void AddRound(float t)
    {
        if (isDiscovered && CheckFunc())
        {
            round += t;

            if (round > activeRound)
            {
                StartCoroutine(TakeEffect());
                lastDeadMonster = null;
                round = 0;
            }
        }
        else
        {
            round = 0;
        }
    }

    public IEnumerator TakeEffect()
    {
        Debug.Log("石碑发动: " + gameObject.name + ", type: " + config.totemType.ToString());

        OnActionBegin();

        switch (config.totemType)
        {
            case TotemType.Protect:

                if (m.Count > 0)
                {
                    int i = Random.Range(0, m.Count);
                    ((Monster)m[i]).AddHpPercent(1);
                }
                break;
            case TotemType.Summon:
    
                if (m.Count < int_arg1)
                {
                    yield return BrickCore.Instance.CreateWhiteMonsterOnRandomStandableBrick();
                }
                break;
            case TotemType.Resurgence:

                BrickCore.Instance.CreateMonsterOnRandomStandableBrick(
                    lastDeadMonster.pwr,
                    lastDeadMonster.lv,
                    lastDeadMonster.config.id);

                break;
            case TotemType.Renew:

                if (m.Count > 1)
                {
                    float maxCurHp = 0;

                    for (int i = 0; i < m.Count; ++i)
                    {
                        if (m[i].cur_hp > maxCurHp)
                        {
                            maxCurHp = m[i].cur_hp;
                        }
                    }

                    for (int i = 0; i < m.Count; ++i)
                    {
                        m[i].cur_hp = maxCurHp;
                    }
                }

                break;
        }

        OnActionEnd();
    }

    public override IEnumerator OnDiscoverd()
    {
        base.OnDiscoverd();

        Messenger<float>.AddListener(SA.StageTimeCast, AddRound);

#if UNITY_EDITOR
        type = config.totemType;
#endif

        switch (config.totemType)
        {
            case TotemType.Protect:
                activeRound = int.Parse(config.arg);
                CheckFunc = VisableMonsterMoreArg;
                break;
            case TotemType.Summon:
                string[] args = config.arg.Split('_');
                activeRound = int.Parse(args[0]);
                int_arg1 = int.Parse(args[1]);
                CheckFunc = VisableMonsterLessArg;
                break;
            case TotemType.Resurgence:
                activeRound = int.Parse(config.arg);
                Messenger<Damage>.AddListener(SA.MonsterDead, OnMonsterDead);
                CheckFunc = HaveMonsterDead;
                break;
            case TotemType.Renew:
                activeRound = int.Parse(config.arg);
                CheckFunc = VisableMonsterMoreArg;
                break;
        }

        return null;
    }


    private bool VisableMonsterMoreArg()
    {
        m = BrickCore.Instance.GetVisableMonsters();

        return m.Count > int_arg1;
    }

    private bool VisableMonsterLessArg()
    {
        m = BrickCore.Instance.GetVisableMonsters();

        return m.Count < int_arg1;
    }

    private bool HaveMonsterDead()
    {
        return lastDeadMonster != null;
    }

    private void OnMonsterDead(Damage damageInfo)
    {
        round = 0;
        lastDeadMonster = damageInfo.damageTarget as Monster;
    }

    public override void Recycle()
    {
        base.Recycle();

        ObjPool<Tablet>.Instance.RecycleObj(GameItemFactory.Instance.tablet_pool, itemId);
        if (config.totemType == TotemType.Resurgence)
        {
            Messenger<Damage>.RemoveListener(SA.MonsterDead, OnMonsterDead);
        }
    }
}

