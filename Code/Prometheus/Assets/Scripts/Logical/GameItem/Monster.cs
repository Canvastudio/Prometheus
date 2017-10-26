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

    /// <summary>
    /// 战斗组件
    /// </summary>
    public FightComponet fightComponet;

    private int player_distance = 0;

    public void Init()
    {
        Messenger.AddListener(SA.PlayerMoveEnd, CheckDistance);
    }

    void OnDisable()
    {
        Messenger.RemoveListener(SA.PlayerMoveEnd, CheckDistance);
    }
   
    public void CheckDistance()
    {
        player_distance = standBrick.pathNode.Distance(StageCore.Instance.Player.standBrick.pathNode);

        if (!isDiscovered && player_distance <= AIConfig.warning)
        {
            OnDiscoverd();
        }

        if (player_distance <= 1)
        {
            if (!block_other)
            {
                BrickCore.Instance.BlockNearbyBrick(standBrick.pathNode.x, standBrick.pathNode.z);
                block_other = true;
            }

            if (AIConfig.dangerous_levels == DangerousLevels.Neutral)
            {
                fightComponet.skillActive = true;
            }
        }

        //ObjPool<Monster>.Instance.RecycleObj(GameItemFactory.Instance.monster_pool, itemId);
    }

    public override void OnDiscoverd()
    {
        base.OnDiscoverd();

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
                    brick.item.OnDiscoverd();
                }
            }
        }


        if (AIConfig.dangerous_levels == DangerousLevels.Hostility)
        {
            fightComponet.skillActive = true;
        }

        if (AIConfig.forceSkills != null)
        {
            var skill_list = AIConfig.forceSkills.ToArray(0);
            StartCoroutine(fightComponet.DoActiveSkill(skill_list));
        }
    }

    public override void OnDead()
    {
        if (standBrick != null)
        {
            BrickCore.Instance.CancelBlockNearbyBrick(standBrick.pathNode.x, standBrick.pathNode.z);
        }
        else
        {
            Debug.LogError("怪物阵亡时发现: standbrick 为空");
        }

        Messenger<Monster>.Invoke(SA.MonsterDead, this);

        base.OnDead();

        if (dead_howl > 0)
        {
            var nearby_list = BrickCore.Instance.GetNearbyNode(standBrick.row, standBrick.column, dead_howl);

            foreach (var n in nearby_list)
            {
                Brick brick = (n.behavirour as Brick);

                if (brick.brickType == BrickType.MONSTER
                    && brick.item.isDiscovered == false)
                {
                    brick.item.OnDiscoverd();
                }

            }
        }
        if (AIConfig.forceSkills != null)
        {
            var skill_list = AIConfig.forceSkills.ToArray(1);
            StartCoroutine(fightComponet.DoActiveSkill(skill_list));
        }
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        fightComponet.skillActive = true;
    }

    public override void Recycle()
    {
        base.Recycle();

        ObjPool<Monster>.Instance.RecycleObj(GameItemFactory.Instance.monster_pool, itemId);
    }
}
