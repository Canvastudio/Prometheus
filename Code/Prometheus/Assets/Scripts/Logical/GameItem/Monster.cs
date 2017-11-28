﻿using System;
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

    private bool block_other = false;

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

    public void CheckDistance()
    {
        player_distance = standBrick.pathNode.Distance(StageCore.Instance.Player.standBrick.pathNode);

        if (!isDiscovered && player_distance <= AIConfig.warning)
        {
            StartCoroutine(standBrick.OnDiscoverd());
        }
        

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
                fightComponet.skillActive = true;
            }
        }

        //ObjPool<Monster>.Instance.RecycleObj(GameItemFactory.Instance.monster_pool, itemId);
    }

    public override IEnumerator OnDiscoverd()
    {
        base.OnDiscoverd();

        RefreshActiveSKillState();
        RefreshPassiveSKillState();

        GContext.Instance.discover_monster += 1;
        GContext.Instance.lastDiscoverMonster = this;

        StageCore.Instance.tagMgr.RemoveEntityTag(this, ETag.Tag(ST.UNDISCOVER));
        StageCore.Instance.tagMgr.AddEntity(this, ETag.Tag(ST.DISCOVER));


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
            fightComponet.skillActive = true;
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

        if (isDiscovered && cur_hp > 0)
        {
            GContext.Instance.discover_monster -= 1;
        }

        Messenger.RemoveListener(SA.PlayerMoveEnd, CheckDistance);
    }
     
    public override IEnumerator OnDead(Damage damageInfo)
    {
        GContext.Instance.discover_monster -= 1;

        if (standBrick != null)
        {
            BrickCore.Instance.CancelBlockNearbyBrick(standBrick.pathNode.x, standBrick.pathNode.z);
        }
        else
        {
            Debug.LogError("怪物阵亡时发现: standbrick 为空");
        }

        Messenger<Damage>.Invoke(SA.MonsterDead, damageInfo);

        if (dead_howl > 0)
        {
            var nearby_list = BrickCore.Instance.GetNearbyLiveItem(standBrick.row, standBrick.column, dead_howl);

            foreach (var n in nearby_list)
            {
                if (!n.isDiscovered)
                {
                    StartCoroutine(n.standBrick.OnDiscoverd());
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

        ObjPool<Monster>.Instance.RecycleObj(GameItemFactory.Instance.monster_pool, itemId);
    }

    public override float TakeDamage(Damage damageInfo)
    {
        fightComponet.skillActive = true;

        return base.TakeDamage(damageInfo);
    }

    public override void Recycle()
    {
        base.Recycle();

        ObjPool<Monster>.Instance.RecycleObj(GameItemFactory.Instance.monster_pool, itemId);
    }
}
