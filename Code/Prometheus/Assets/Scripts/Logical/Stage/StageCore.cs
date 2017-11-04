using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCore : SingleObject<StageCore> {

    List<GameItemBase> allItems = new List<GameItemBase>();

    delegate IEnumerator LoopAction();

    Queue<LoopAction> activeQueue = new Queue<LoopAction>();

    public Player Player;

    ulong monsterId = 0;

    bool isMonsterActionFilish = true;

    bool inputConfrim;

    WaitUntil AllActionFinish;

    WaitForMsg waitMsg = new WaitForMsg();

    /// <summary>
    /// 第一次点击的方块
    /// </summary>
    Brick brick1;
    /// <summary>
    /// 第二次点击的方块（通常用于确认）
    /// </summary>
    Brick brick2;

    /// <summary>
    /// 当前到了第几回合
    /// </summary>
    public float totalTime;

    public bool isLooping = false;

    /// <summary>
    /// 时间
    /// </summary>
    public float turnTime = 0;

    /// <summary>
    /// 实例标签管理
    /// </summary>
    public EntitysTag<GameItemBase> tagMgr;

    /// <summary>
    /// 关卡记录,保存一些之后需要读取的信息
    /// </summary>
    public StageRecording records;

    /// <summary>
    /// 玩家是处于动作中
    /// </summary>
    public bool playerDoing = false;

    /// <summary>
    /// 是否符合被动里面的just状态
    /// </summary>
    private bool inJustState = false;

    /// <summary>
    /// 最后一个翻开的怪物
    /// </summary>
    private Monster lastDiscoverMonster = null;
    public bool JustdiscoverMonster = false;

    public void SetDiscoverMonster(Monster monster)
    {
        lastDiscoverMonster = monster;
        JustdiscoverMonster = true;
    }

    protected override void Init()
    {
        base.Init();

        tagMgr = new EntitysTag<GameItemBase>();
        records = new StageRecording();

        AllActionFinish = new WaitUntil(() => isMonsterActionFilish);

        //监听一些重要事件
        Messenger<Damage>.AddListener(SA.MonsterDead, OnMonsterDead);

        CoroCore.Instance.ExStartCoroutine(CheckTurnTime());
    }

    public void RegisterItem(GameItemBase gameItemBase)
    {
        tagMgr.AddEntity(gameItemBase
            , ETag.GetETag(gameItemBase.GetType().ToString()));

        if (gameItemBase is Player)
        {
            Player = gameItemBase as Player;
        }
        else if (gameItemBase is Monster)
        {
            tagMgr.AddEntity(gameItemBase, ETag.GetETag(ST.ENEMY, ST.UNDISCOVER));
        }

        allItems.Add(gameItemBase);
    }

    public int discover_monster = 0;

    public void UnRegisterItem(GameItemBase gameItem)
    {
        allItems.Remove(gameItem);
        tagMgr.RemoveEntity(gameItem);
    }

    public void OnMonsterDead(Damage damageInfo)
    {
        if (records.lastDeadMonster == null)
            records.lastDeadMonster = new StageRecording.DeadMonsterRecord();

        var monster = damageInfo.damageTarget as Monster;
        records.lastDeadMonster.brick = monster.standBrick;
        records.lastDeadMonster.lv = monster.lv;
        records.lastDeadMonster.pwr = monster.pwr;
        records.lastDeadMonster.uid = monster.cid;
    }

    public IEnumerator DoNearByBrickSpecialAction(Brick brick1)
    {
        var type = brick1.brickType;

        if (type == BrickType.MONSTER)
        {
            yield return PlayerMeleeAction(brick1);
        }
        else if (type == BrickType.SUPPLY)
        {
            var item = brick1.item as Supply;
            yield return Player.moveComponent.MoveTo(brick1, 0.3f);

            //吃掉
            item.Reactive();
        }
        else if (type == BrickType.TREASURE)
        {
            var item = brick1.item as Treasure;

            yield return Player.moveComponent.MoveTo(brick1, 0.3f);

            //吃掉
            item.Reactive();
        }
    }

    /// <summary>
    /// 关卡内部逻辑循环
    /// </summary>
    /// <returns></returns>
    public IEnumerator RunLoop()
    {
        isLooping = true;

        while (isLooping)
        {
            inputConfrim = false;

            //如果执行队列还有东西，那么自动执行
            if (activeQueue.Count > 0)
            {
                yield return activeQueue.Dequeue().Invoke();
            }
            else
            {
                StageView.Instance.CancelPahtNode();

                //如果没有处于自动状态，则等待并处理玩家点击事件
                yield return waitMsg.BeginWaiting<Brick>(SA.PlayerClickBrick).BeginWaiting<SkillListItem>(SA.PlayerClickSkill);

                if (waitMsg.result.msg == SA.PlayerClickBrick)
                {
                    brick1 = waitMsg.result.para as Brick;

                    if (brick1 != Player.standBrick)
                    {
                        //事件返回 从事件中得到参数

                        BrickType type = brick1.brickType;

                        BrickLogical:

                        //停止监听砖块点击事件
                        switch (type)
                        {
                            case BrickType.EMPTY:
                            case BrickType.UNKNOWN:
                            case BrickType.MONSTER:
                            case BrickType.SUPPLY:
                            case BrickType.TREASURE:

                                int d = Player.standBrick.pathNode.Distance(brick1.pathNode);

                                bool need_action = false;

                                if (d == 1 && type != BrickType.EMPTY && type != BrickType.UNKNOWN)
                                {
                                    yield return DoNearByBrickSpecialAction(brick1);
                                }
                                else
                                {
                                    List<Pathfinding.Node> list = null;

                                    if (type != BrickType.EMPTY && type != BrickType.UNKNOWN)
                                    {
                                        list = Pathfinding.PathfindMaster.Instance.RequestShortestPathToNeigbour(brick1.pathNode, Player.standBrick.pathNode, BrickCore.Instance);
                                        need_action = true;
                                    }
                                    else
                                    {
                                        //开始计算路径
                                        list = Pathfinding.PathfindMaster.Instance.RequestPathfind(Player.standBrick.pathNode, brick1.pathNode, BrickCore.Instance);
                                    }

                                    //标记路径
                                    StageView.Instance.CancelPahtNode();
                                    StageView.Instance.SetNodeAsPath(list);
                                    //如果路径长度小于3，不需要确认直接移动
                                    if (list.Count < 3)
                                    {
                                        Player.moveComponent.SetPaht(list);

                                        yield return MovePlayer();

                                        if (need_action)
                                            yield return DoNearByBrickSpecialAction(brick1);
                                    }
                                    else
                                    {
                                        //等待玩家点击确认
                                        yield return waitMsg.BeginWaiting<Brick>(SA.PlayerClickBrick.ToString());

                                        brick2 = waitMsg.result.para as Brick;

                                        if (brick1 == brick2)
                                        {
                                            //确认成功
                                            //yield return StageCore.Instance.Player.moveComponent.Go(list);
                                            Player.moveComponent.SetPaht(list);

                                            yield return MovePlayer();

                                            if (need_action)
                                                yield return DoNearByBrickSpecialAction(brick1);
                                        }
                                        else
                                        {
                                            brick1 = brick2;
                                            goto BrickLogical;
                                        }
                                    }



                                }
                                break;
                        }
                    }
                }
                else if (waitMsg.result.msg == SA.PlayerClickSkill)
                {
                    if (!Player.isDisarm)
                    {
                        ulong skill_id = (waitMsg.result.para as SkillListItem).skill_id;

                        Debug.Log("使用技能id: " + skill_id);

                        yield return Player.fightComponet.DoActiveSkill(ConfigDataBase.GetConfigDataById<ActiveSkillsConfig>(skill_id));
                    }
                }
            }

            //等待玩家动作结束信号
            yield return AllActionFinish;
        }
    }

    IEnumerator MovePlayer()
    {
        while (!Instance.Player.moveComponent.MoveEnd())
        {
            yield return StageCore.Instance.Player.moveComponent.MoveToNext();
            Messenger.Invoke(SA.PlayerMoveEnd);
        }

        //TODO: 触发非空砖块

        //autoMove = false;
        StageView.Instance.CancelPahtNode();

        JustdiscoverMonster = false;
    }
    
    IEnumerator PlayerMeleeAction(Brick brick1)
    {
        bool just = false;

        var monster = brick1.item as Monster;

        if (monster.itemId == lastDiscoverMonster.itemId && JustdiscoverMonster)
        {
            just = true;
        }

        var player_Speed = Player.Property.GetFloatProperty(GameProperty.speed);
        var monster_Speed = monster.Property.GetFloatProperty(GameProperty.speed);

        if (player_Speed >= monster_Speed || monster.enslave)
        {
            yield return Player.MeleeAttackTarget(monster);

            if (monster != null && monster.isAlive && !monster.enslave)
            {
                yield return monster.MeleeAttackTarget(Player);
            }

            if (Player != null && Player.isAlive && player_Speed >= monster_Speed * 1.5f)
            {
                yield return Player.MeleeAttackTarget(monster);
            }
        }
        else
        {
            yield return monster.MeleeAttackTarget(Player);

            if (Player != null && Player.isAlive)
            {
                yield return Player.MeleeAttackTarget(monster);
            }

            if (monster != null && monster.isAlive && monster_Speed >= player_Speed * 1.5f)
            {
                yield return monster.MeleeAttackTarget(Player);
            }
        }

        if (just)
        {

        }

        JustdiscoverMonster = false;
    }

    public IEnumerator StopLoop()
    {
        //停止逻辑循环
        CoroCore.Instance.ExStartCoroutine(RunLoop());
        //重置标志
        isLooping = false;

        yield return 0;
    }

     
    public void TimeCast(float time)
    {
        turnTime += time;

        totalTime += time;
    }

    public IEnumerator CheckTurnTime()
    {
        while (true)
        {
            if (turnTime > 0)
            {
                turnTime = Mathf.Max(0, turnTime - Time.deltaTime);
                TimeCastSoMoveDown(Time.deltaTime);
                Messenger<float>.Invoke(SA.StageTimeCast, Time.deltaTime);
            }

            yield return 0;
        }
    }

    public void TimeCastSoMoveDown(float time)
    {
        StageView.Instance.MoveDownMap(time);
    }

    public void  GetMonsterInRange(Brick stand_brick, int range, ref List<Monster> list)
    {
        tagMgr.GetEntity(ref list, ETag.GetETag(ST.DISCOVER, ST.MONSTER));

        for (int i = list.Count - 1; i >= 0; --i)
        {
            if (list[i].standBrick.pathNode.Distance(stand_brick.pathNode) > range)
            {
                list.RemoveAt(i);
            }
        }
    }
}



public static class SA
{
    public const string PlayerClickBrick = "PCB";
    public const string PlayerClickSkill = "PCS";
    public const string RefreshGameItemPos = "RGIP";
    public const string StageTimeCast = "STC";
    public const string MapMoveDown = "MMD";
    public const string MonsterDead = "MSD";
    public const string StuffCountChange = "SCC";
    public const string PlayerMoveEnd = "PME";
    public const string PlayerMoveStep = "PMS";
    public const string PlayerUseSkill = "PUS";
    public const string ItemTakeDamage = "ITD";
}

public static class ST
{
    public const string BRICK = "Brick";//类型名
    public const string MONSTER = "Monster"; //类型名
    public const string PLAYER = "Player"; //类型名
    public const string OBSTACLE = "Obstacle";

    public const string ENEMY = "EY";
    public const string DISCOVER = "DR";
    public const string UNDISCOVER = "UDR";
    public const string FRIEND = "FD";
    public const string LAST = "LT";
}



