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
    public MonsterType monsterType;
    /// <summary>
    /// 策划属性配置表
    /// </summary>
    public MonsterConfig config;

    /// <summary>
    /// 当前怪物强度
    /// </summary>
    public int pwr;
    /// <summary>
    /// 当前怪物等级
    /// </summary>
    public int lv;
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


    public void CheckDistance()
    {
        player_distance = standBrick.pathNode.Distance(StageCore.Instance.Player.standBrick.pathNode);

        if (AIConfig.warning > 0)
        {
            if (!isDiscovered && player_distance <= AIConfig.warning)
            {
                standBrick.OnDiscoverd();
            }
        }

        if (player_distance <= 1)
        {
            if (!block_other)
            {
                BrickCore.Instance.BlockNearbyBrick(standBrick.pathNode.x, standBrick.pathNode.z);
                block_other = true;
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

        RefreshPassiveSKillState();

        GContext.Instance.discover_monster += 1;
        GContext.Instance.lastDiscoverMonster = this;

        StageCore.Instance.tagMgr.RemoveEntityTag(this, ETag.Tag(ST.UNDISCOVER));
        StageCore.Instance.tagMgr.AddEntity(this, ETag.Tag(ST.DISCOVER));


        if (standBrick != null)
        {
            if (player_distance <= 1)
            {
                if (!block_other)
                {
                    Debug.Log("发现: " + gameObject.name);
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
            var nearby_list = BrickCore.Instance.GetNearbyNode(standBrick.row, standBrick.column, discover_howl);

            foreach(var n in nearby_list)
            {
                Brick brick = (n.behavirour as Brick);

                if (brick.brickType == BrickType.MONSTER
                    && brick.item.isDiscovered == false)
                {
                    yield return brick.item.OnDiscoverd();
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
            StartCoroutine(fightComponet.DoActiveSkill(skill_list));
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
            var nearby_list = BrickCore.Instance.GetNearbyNode(standBrick.row, standBrick.column, dead_howl);

            foreach (var n in nearby_list)
            {
                Brick brick = (n.behavirour as Brick);

                if (brick.brickType == BrickType.MONSTER
                    && brick.item.isDiscovered == false)
                {
                    StartCoroutine(brick.OnDiscoverd());
                }

            }
        }

        if (AIConfig.forceSkills != null)
        {
            var skill_list = AIConfig.forceSkills.ToArray(1);

            if (skill_list[0] > 0)
            {
                yield return fightComponet.DoActiveSkill(skill_list);
            }
        }

        base.OnDead(damageInfo);

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
