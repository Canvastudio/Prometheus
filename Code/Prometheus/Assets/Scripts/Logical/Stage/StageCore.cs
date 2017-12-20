using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCore : SingleGameObject<StageCore> {

    public bool gaming = false;

    List<GameItemBase> allItems = new List<GameItemBase>();

    delegate IEnumerator LoopAction();

    Queue<LoopAction> activeQueue = new Queue<LoopAction>();

    public Player Player;

    public int action_item = 0;

    WaitUntil AllActionFinish;

    WaitForMsg waitMsg = new WaitForMsg();

    public bool monsterwakeup = false;

    public int playerDistance = 0;

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

    protected override void Init()
    {
        base.Init();

        tagMgr = new EntitysTag<GameItemBase>();
        records = new StageRecording();

        AllActionFinish = new WaitUntil(() => 
        {
            return CanGoing();
        });

        //监听一些重要事件
        Messenger<Damage>.AddListener(SA.MonsterDead, OnMonsterDead);

        CoroCore.Instance.ExStartCoroutine(CheckTurnTime());
    }
    
    public bool CanGoing()
    {
        return action_item == 0 && GCamera.Instance.sliding == false;
    }

    public void RegisterItem(GameItemBase gameItemBase)
    {
        if (gameItemBase is Player)
        {
            Player = gameItemBase as Player;
        }
        else if (gameItemBase is Monster)
        {
            Monster monster = gameItemBase as Monster;

            if (monster.Side == LiveItemSide.SIDE0)
            {
                tagMgr.AddEntity(gameItemBase, ETag.GetETag(ST.MONSTER, ST.SIDE0, ST.UNDISCOVER));
            }
            else
            {
                tagMgr.AddEntity(gameItemBase, ETag.GetETag(ST.MONSTER, ST.SIDE1, ST.UNDISCOVER));
            }
        }
        else
        {
            if (gameItemBase is Brick)
            {
                tagMgr.AddEntity(gameItemBase, ETag.GetETag(ST.BRICK, ST.UNDISCOVER));
            }
            else
            {
                tagMgr.AddEntity(gameItemBase, ETag.GetETag(gameItemBase.GetType().ToString()));
            }
        }

        allItems.Add(gameItemBase);
    }

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

        if (type == BrickType.MONSTER && Instance.Player.standBrick.pathNode.Distance(brick1.pathNode) == 1)
        {
            yield return PlayerMeleeAction(brick1);
        }
        else if (type == BrickType.SUPPLY)
        {
            var item = brick1.item as Supply;

            yield return Player.moveComponent.MoveTo(brick1, Rpn.GetMoveTime());

            //吃掉
            item.Reactive();
        }
        else if (type == BrickType.TREASURE)
        {
            var item = brick1.item as Treasure;

            yield return Player.moveComponent.MoveTo(brick1, Rpn.GetMoveTime());

            //吃掉
            item.Reactive();
        }
        else if (type == BrickType.MAINTENANCE)
        {
            TimeCast(Rpn.GetMoveTime());

            var item = brick1.item as Maintenance;

            //吃掉
            item.Reactive();
        }
        else if (type == BrickType.Organ)
        {
            TimeCast(Rpn.GetMoveTime());

            (brick1.item as OrganBase).Reactive();
        }
        

    }

    /// <summary>
    /// 关卡内部逻辑循环
    /// </summary>
    /// <returns></returns>
    public IEnumerator RunLoop()
    {
        Messenger.Invoke(SA.GameStart);

        StageUIView.Instance.upUIView.RefreshHpUI();
        StageUIView.Instance.IniMat();
        List<Pathfinding.Node> list1 = new List<Pathfinding.Node>();
        yield return Player.standBrick.OnDiscoverd();

        isLooping = true;

        gaming = true;

        while (isLooping)
        {
            //如果执行队列还有东西，那么自动执行
            if (activeQueue.Count > 0)
            {
                yield return activeQueue.Dequeue().Invoke();
            }
            else
            {
                StageView.Instance.CancelPahtNode();

                Debug.Log("主循环等待中......");
                //如果没有处于自动状态，则等待并处理玩家点击事件
                yield return waitMsg.BeginWaiting<Brick>(SA.PlayerClickBrick).BeginWaiting<ActiveSkillIns>(SA.PlayerClickActiveSkill);

                Debug.Log("主循执行.....: " + waitMsg.result.msg);

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
                            case BrickType.SUPPLY:
                                Supply item = brick1.item as Supply;
                                item.Reactive();
                                break;
                            case BrickType.TREASURE:
                                Treasure _item = brick1.item as Treasure;
                                _item.Reactive();
                                break;
                            case BrickType.OrganProperty:
                                if (brick1.item is PropertyOrgan)
                                {
                                    PropertyOrgan __item = brick1.item as PropertyOrgan;
                                    __item.Reactive();
                                }
                                break;
                            case BrickType.EMPTY:
                            case BrickType.UNKNOWN:
                            case BrickType.MONSTER:
                            //case BrickType.SUPPLY:
                            //case BrickType.TREASURE:
                            case BrickType.MAINTENANCE:
                            default:            
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

                                    if (list != null && list.Count > 0)
                                    {

                                        list1.Clear();
                                        list1.AddRange(list);
                                        list1.Add(brick1.pathNode);

                                        //标记路径
                                        StageView.Instance.CancelPahtNode();
                                        StageView.Instance.SetNodeAsPath(list1);

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
                                            yield return waitMsg.BeginWaiting<Brick>(SA.PlayerClickBrick.ToString()).BeginWaiting<ActiveSkillsConfig>(SA.PlayerClickActiveSkill);

                                            if (waitMsg.result.msg == SA.PlayerClickBrick)
                                            {
                                                brick2 = waitMsg.result.para as Brick;

                                                if (brick1 == brick2)
                                                {
                                                    //确认成功
                                                    //yield return StageCore.Instance.Player.moveComponent.Go(list);
                                                    Player.moveComponent.SetPaht(list);

                                                    yield return MovePlayer();

                                                    if (need_action && Player.standBrick.pathNode.Distance(brick1.pathNode) == 1)
                                                        yield return DoNearByBrickSpecialAction(brick1);
                                                }
                                                else
                                                {
                                                    brick1 = brick2;
                                                    goto BrickLogical;
                                                }
                                            }
                                            else if (waitMsg.result.msg == SA.PlayerClickActiveSkill)
                                            {
                                                StageView.Instance.CancelPahtNode();

                                                var ins = (waitMsg.result.para as ActiveSkillIns);

                                                yield return DoPlayerSkill(ins);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Debug.Log("青鑫：无法到达指定的位置!");
                                    }
                                }

                                break;
                        }
                    }
                }
                else if (waitMsg.result.msg == SA.PlayerClickActiveSkill)
                {
                    if (Instance.Player.Disarm == 0)
                    {
                        var ins = (waitMsg.result.para as ActiveSkillIns);
                        yield return DoPlayerSkill(ins);
                        Debug.Log("技能结束, 回到主循环..");
                    }
                }

            }

            //等待玩家动作结束信号
            yield return AllActionFinish;

            monsterwakeup = false;
        }
    }

    IEnumerator DoPlayerSkill(ActiveSkillIns ins)
    {
        yield return Player.fightComponet.DoActiveSkill(ins);
    }

    IEnumerator MovePlayer()
    {
        StageView.Instance.CancelPahtNode();

        GContext.Instance.JustdiscoverMonster = false;

        while (!Instance.Player.moveComponent.MoveEnd() && ! monsterwakeup)
        {
            yield return StageCore.Instance.Player.moveComponent.MoveToNext();

            Messenger.Invoke(SA.PlayerMoveEnd);
        }

        //TODO: 触发非空砖块

        //autoMove = false;
        StageView.Instance.CancelPahtNode();


    }

    WaitForSeconds w02s = new WaitForSeconds(.2f);
    WaitForSeconds w05s = new WaitForSeconds(.5f);

    IEnumerator PlayerMeleeAction(Brick brick1)
    {
        bool just = false;

        var monster = brick1.item as Monster;

        Just ins = null;

        if (GContext.Instance.JustdiscoverMonster && monster.itemId == GContext.Instance.lastDiscoverMonster.itemId)
        {
            just = true;

            foreach (var s in Player.state.state_list)
            {
                foreach (var ss in s.stateEffects)
                {
                    if (ss.stateType == StateEffectType.JustPropertyChange && ss.active)
                    {
                        ins = ss as Just;
                        ins.ApplyChange();
                    }
                }
            }
        }

        var player_Speed = Player.Property.GetFloatProperty(GameProperty.speed);
        var monster_Speed = monster.Property.GetFloatProperty(GameProperty.speed);

        if (player_Speed >= monster_Speed || monster.enslave)
        {
            var config = ConfigDataBase.GetConfigDataById<GlobalParameterConfig>(1);
            float[] f;
            float timeSpend = Rpn.CalculageRPN(config.atkSpeedFormula.ToArray(), Instance.Player, null, out f);

            StageCore.Instance.TimeCast(timeSpend);
            yield return Player.MeleeAttackTarget(monster);

            if (monster != null && monster.cur_hp > 0 && !monster.enslave)
            {
                yield return monster.MeleeAttackTarget(Player);
            }

            if (Player != null && Player.cur_hp > 0 && player_Speed >= monster_Speed * 1.5f)
            {
                yield return w05s;
                yield return Player.MeleeAttackTarget(monster);
            }
        }
        else
        {
            yield return monster.MeleeAttackTarget(Player);

            if (Player != null && Player.cur_hp > 0)
            {
                var config = ConfigDataBase.GetConfigDataById<GlobalParameterConfig>(1);
                float[] f;
                float timeSpend = Rpn.CalculageRPN(config.atkSpeedFormula.ToArray(), Instance.Player, null, out f);

                StageCore.Instance.TimeCast(timeSpend);

                yield return Player.MeleeAttackTarget(monster);
            }

            if (monster != null && monster.cur_hp > 0 && monster_Speed >= player_Speed * 1.5f)
            {
                yield return w05s;
                yield return monster.MeleeAttackTarget(Player);
            }
        }

        if (just && ins != null)
        {
            ins.ResetChange();
        }

        GContext.Instance.JustdiscoverMonster = false;

        if (!monster.isAlive)
        {
            yield return w05s;
        }
    }

    public IEnumerator StopLoop()
    {
        //停止逻辑循环
        CoroCore.Instance.StopCoroutine(RunLoop());
        //重置标志
        isLooping = false;

        yield return 0;
    }

     
    public void TimeCast(float time)
    {
        turnTime += time;
        totalTime += time;

        StageView.Instance.MoveDownMap(time);
    }

    public IEnumerator CheckTurnTime()
    {
        while (true)
        {
            if (turnTime > 0)
            {
                turnTime = Mathf.Max(0, turnTime - Time.deltaTime);
                //StageView.Instance.MoveDownMap(Time.deltaTime);
                Messenger<float>.Invoke(SA.StageTimeCast, Time.deltaTime);
            }

            yield return 0;
        }
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
    public const string PlayerClickActiveSkill = "PCS";
    public const string RefreshGameItemPos = "RGIP";
    public const string StageTimeCast = "STC";
    public const string MapMoveDown = "MMD";
    public const string MonsterDead = "MSD";
    public const string StuffCountChange = "SCC";
    public const string PlayerMoveEnd = "PME";
    public const string PlayerMoveStep = "PMS";
    public const string LiveUseSkill = "PUS";
    public const string LivePreuseSkill = "PRS";
    public const string ItemTakeDamage = "ITD";

    public const string PlayHpChange = "PIC";
    public const string PlayerAddState = "PAS";
    public const string PlayerRemoveState = "PRS";

    public const string EnmeyCountChange = "ECC";
    public const string DiscoverMonsterChange = "DMC";

    public const string DiscoverBrickChange = "DBC";
    public const string OpenBrick = "OBK";
    public const string DarkBrickChange = "DKBC";

    public const string GameStart = "GST";
    public const string GameEnd = "GSE";

    public const string CamerMove = "CME";
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

    public const string SIDE0 = "SIDE0";
    public const string SIDE1 = "SIDE1";
}



