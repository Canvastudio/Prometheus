using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 存储怪物信息
/// </summary>
public class Monster : LiveItem
{
    /// <summary>
    /// 策划属性配置表
    /// </summary>
    public MonsterConfig config;

    /// <summary>
    /// 当前怪物等级
    /// </summary>
    public int lv;


    /// <summary>
    /// 怪物名字，测试用
    /// </summary>
    public Text monsterName;

    /// <summary>
    /// 当前怪物在表中的id
    /// </summary>
    public ulong cid;

    /// <summary>
    /// 行为逻辑配置表
    /// </summary>
    public AIConfig AIConfig;

    public bool block_other = false;

    public int discover_howl;

    public int dead_howl;

    private int player_distance = 0;

    [SerializeField]
    public Image pwrFrame;

    /// <summary>
    /// 是否被玩家奴役
    /// </summary>
    [SerializeField]
    private bool _enslave;
    public bool enslave
    {
        get
        {
            return _enslave;
        }
        set
        {
            if (_enslave != value)
            {
                StageCore.Instance.tagMgr.SetEntityTag(this, ETag.Tag(ST.FRIEND), value);
                StageCore.Instance.tagMgr.SetEntityTag(this, ETag.Tag(ST.ENEMY), !value);

                if (enslave)
                {
                    Side = LiveItemSide.SIDE0;
                    GContext.Instance.enslave_count += 1;
                }
                else
                {
                    Side = LiveItemSide.SIDE1;
                    GContext.Instance.enslave_count -= 1;
                }

                _enslave = value;
            }
        }
    }

    public DangerousLevels dangerousLevels;

    public ArtPop artPop;

    private void OnEnable()
    {
        //icon.gameObject.SetActive(false);
    }

    public override void AddStateUI(StateIns ins)
    {
        base.AddStateUI(ins);

        if (ins.stateConfig.iShow)
        {
            artPop.Add(StageView.Instance.stateAtlas.GetSprite(ins.stateConfig.icon));
        }
    }

    public override void RemoveStateUI(StateIns ins)
    {
        base.RemoveStateUI(ins);

        if (ins.stateConfig.iShow)
        {
            artPop.Remove(StageView.Instance.stateAtlas.GetSprite(ins.stateConfig.icon));
        }
    }

    public void CheckDistance()
    {
        if (!gameObject.activeInHierarchy)
        {
            Debug.Log("没有激活的物体参与了checkDistance? : " + gameObject.name);
        }

        player_distance = standBrick.pathNode.Distance(StageCore.Instance.Player.standBrick.pathNode);

        if (!isDiscovered && player_distance <= AIConfig.warning)
        {
            StageCore.Instance.monsterwakeup = true;
            ///因为警戒而翻开
            StartCoroutine(standBrick.OnDiscoverd());
        }
        

        ///如果贴身了 就执行阻挡
        if (player_distance <= 1 && isDiscovered)
        {
            if (!block_other)
            {
                BrickCore.Instance.BlockNearbyBrick(standBrick.pathNode.x, standBrick.pathNode.z);
                block_other = true;

                if (!isDiscovered)
                {
                    StartCoroutine(standBrick.OnDiscoverd());
                }
            }

            if (dangerousLevels == DangerousLevels.Neutral)
            {
                fightComponet.ActiveSkill();
            }
        }

        //ObjPool<Monster>.Instance.RecycleObj(GameItemFactory.Instance.monster_pool, itemId);
    }

    protected override void OnSetStandBrick(Brick brick)
    {
        base.OnSetStandBrick(brick);

        brick.brickType = BrickType.MONSTER;
    }

    WaitForSeconds wait05s = new WaitForSeconds(0.5f);

    public override IEnumerator OnDiscoverd()
    {
        ////StartCoroutine(StageView.Instance.ShowFx(standBrick, "怪物翻开"));
        //string name = SpecialEffectConfig.GetConfigDataByKey<SpecialEffectConfig>("怪物翻开").effectName;
        //ArtSkill.Show(name, transform.position);

        //yield return wait05s;

        //icon.gameObject.SetActive(true);
        transform.SetParent(StageView.Instance.top, true);

        base.OnDiscoverd();

        RefreshActiveSKillState();
        RefreshPassiveSKillState();

        foreach(var state in state.state_list)
        {
            if (!state.active)
            {
                state.ActiveIns();
            }
        }

        GContext.Instance.discover_monster += 1;
        GContext.Instance.lastDiscoverMonster = this;

        player_distance = standBrick.pathNode.Distance(StageCore.Instance.Player.standBrick.pathNode);

        if (standBrick != null)
        {
            if (player_distance <= 1)
            {
                if (!block_other)
                {
                    BrickCore.Instance.BlockNearbyBrick(standBrick.pathNode.x, standBrick.pathNode.z);
                    block_other = true;
                }
            }
        }

        var noise = AIConfig.noise.ToArray();
        discover_howl = noise[0];
        dead_howl = noise[1];

        if (discover_howl > 0)
        {
            Debug.Log("Monster discover_howl: " + discover_howl);
            //tartCoroutine(StageView.Instance.ShowFx(standBrick, "叫醒"));
            string fname = SpecialEffectConfig.GetConfigDataByKey<SpecialEffectConfig>("叫醒").effectName;
            ArtSkill.Show(fname, transform.position, dead_howl);
            var nearby_list = BrickCore.Instance.GetNearbyLiveItem(standBrick.row, standBrick.column, discover_howl);

            foreach(var n in nearby_list)
            {
                if (!n.isDiscovered)
                {
                    yield return n.standBrick.item.OnDiscoverd();
                }
            }
        }

        if (dangerousLevels == DangerousLevels.Hostility)
        {
            fightComponet.ActiveSkill();
        }

        if (AIConfig.forceSkills != null)
        {
            var skill_list = AIConfig.forceSkills.ToArray(0);
            if (skill_list[0] > 0)
            {
                Debug.Log("被发现时怪物： " + gameObject.name + " 释放技能: " + skill_list[0]);
                ActiveSkillsConfig config = ConfigDataBase.GetConfigDataById<ActiveSkillsConfig>(skill_list[0]);
                StartCoroutine(fightComponet.DoActiveSkill(config));
            }
        }
    }

    protected override void OnEnterIntoArea()
    {
        base.OnEnterIntoArea();

        Messenger.AddListener(SA.PlayerMoveEnd, CheckDistance);
    }

    protected override void OnExitFromArea()
    {
        base.OnExitFromArea();

        Messenger.RemoveListener(SA.PlayerMoveEnd, CheckDistance);
    }
     
    public override IEnumerator OnDead(Damage damageInfo)
    {


        if (standBrick != null)
        {
            BrickCore.Instance.CancelBlockNearbyBrick(standBrick.pathNode.x, standBrick.pathNode.z);
        }
        else
        {
            Debug.LogError("怪物阵亡时发现: standbrick 为空");
        }

        if (dead_howl > 0)
        {
            //StartCoroutine(StageView.Instance.ShowFx(standBrick, "叫醒"));
            string fname = SpecialEffectConfig.GetConfigDataByKey<SpecialEffectConfig>("叫醒").effectName;
            ArtSkill.Show(fname, transform.position, dead_howl);
            yield return wait05s;

            var nearby_list = BrickCore.Instance.GetNearbyLiveItem(standBrick.row, standBrick.column, dead_howl);
             
            foreach (var n in nearby_list)
            {
                if (!n.isDiscovered) if(n.isAlive)
                {
                    if (n.isAlive)
                        StartCoroutine(n.standBrick.OnDiscoverd());
                }
                else
                {
                    Debug.Log("一个死亡的怪物 但是brick缺米有打开？");
                }

            }
        }

        if (AIConfig.forceSkills != null)
        {
            var skill_list = AIConfig.forceSkills.ToArray(1);
            if (skill_list[0] > 0)
            {
                Debug.Log("死亡时怪物： " + gameObject.name + " 释放技能: " + skill_list[0]);
                ActiveSkillsConfig config = ConfigDataBase.GetConfigDataById<ActiveSkillsConfig>(skill_list[0]);
                yield return (fightComponet.DoActiveSkill(config));
            }
        }

        yield return base.OnDead(damageInfo);

        Debug.Log("怪物死亡：" + gameObject.name);

        Messenger<Damage>.Invoke(SA.MonsterDead, damageInfo);

        //StartCoroutine(StageView.Instance.ShowFx(standBrick, "怪物死亡"));
        string ffname = SpecialEffectConfig.GetConfigDataByKey<SpecialEffectConfig>("怪物死亡").effectName;
        ArtSkill.Show(ffname, transform.position);
        yield return wait05s;

        Recycle();
    }

    public override float TakeDamage(Damage damageInfo)
    {
        fightComponet.ActiveSkill();

        return base.TakeDamage(damageInfo);
    }

    public override void Recycle()
    {
        if (isDiscovered)
        {
            GContext.Instance.discover_monster -= 1;
        }

        base.Recycle();

        enslave = false;
        isAlive = false;
        fightComponet.Clean();
        block_other = false;
        Messenger.RemoveListener(SA.PlayerMoveEnd, CheckDistance);
        ObjPool<Monster>.Instance.RecycleObj(GameItemFactory.Instance.monster_pool, itemId);
    }
}
