using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCore : SingleObject<StageCore> {

    Dictionary<ulong, Monster> monsterDic = new Dictionary<ulong, Monster>();

    public Player Player;

    ulong monsterId = 0;

    bool isPlayerActionFilish;
    bool inputConfrim;

    WaitUntil playerActionFinish;
    Messenger<Brick>.WaitForMsg brickMsg;
    /// <summary>
    /// 第一次点击的方块
    /// </summary>
    Brick brick1;
    /// <summary>
    /// 第二次点击的方块（通常用于确认）
    /// </summary>
    Brick brick2;
    /// <summary>
    /// 玩家角色正在自动执行动作，自动寻路进行移动
    /// 时间消耗 会造成 格子下移
    /// </summary>
    bool autoMove;

    /// <summary>
    /// 当前到了第几回合
    /// </summary>
    public int totalRound;

    public bool isLooping = false;

    /// <summary>
    /// 当前回合时间消耗，这个消耗不总是等于1
    /// </summary>
    public float turnTime = 0;

    protected override void Init()
    {
        base.Init();

        playerActionFinish = new WaitUntil(() => isPlayerActionFilish);

        brickMsg = new Messenger<Brick>.WaitForMsg();
    }

    public void RegisterMonster(Monster newMonster)
    {
        newMonster.uid = monsterId++;

        monsterDic.Add(newMonster.uid, newMonster);
    }

    public void RegisterPlayer(Player player)
    {
        Player = player;
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
            isPlayerActionFilish = false;
            inputConfrim = false;

            if (autoMove)
            {
                yield return MovePlayer();
            }
            else
            {
                while (!isPlayerActionFilish)
                {
                    StageView.Instance.CancelPahtNode();

                    //如果没有处于自动状态，则等待并处理玩家点击事件
                    yield return brickMsg.BeginWaiting(StageAction.PlayerClickBrick.ToString());

                    //事件返回 从事件中得到参数
                    brick1 = brickMsg.para;

                    BrickLogical:

                    //停止监听砖块点击事件
                    switch (brick1.brickType)
                    {
                        case BrickType.EMPTY:
                        case BrickType.UNKNOWN:

                            //开始计算路径
                            var list = Pathfinding.PathfindMaster.Instance.RequestPathfind(Player.standBrick.pathNode, brick1.pathNode, BrickCore.Instance);
                            //标记路径
                            StageView.Instance.SetNodeAsPath(list);
                            //如果路径长度小于3，不需要确认直接移动
                            if (list.Count < 3)
                            {
                                Player.moveComponent.SetPaht(list);

                                yield return MovePlayer();

                                //yield return Player.moveComponent.Go(list);
                            }
                            else
                            {
                                //等待玩家点击确认
                                yield return brickMsg.BeginWaiting(StageAction.PlayerClickBrick.ToString());

                                brick2 = brickMsg.para;
                                
                                if (brick1 == brick2)
                                {
                                    //确认成功
                                    //yield return StageCore.Instance.Player.moveComponent.Go(list);
                                    Player.moveComponent.SetPaht(list);

                                    yield return MovePlayer();
                                }
                                else
                                {
                                    brick1 = brick2;
                                    goto BrickLogical;
                                }
                            }
                            break;
                        case BrickType.TREASURE:
                            Debug.Log("玩家点击了宝藏！");

                            if (Player.standBrick.pathNode.Distance(brick1.pathNode) == 1)
                            {
                                var item = brick1.item as Treasure;

                                item.Reactive();
                            }

                            break;
                        case BrickType.MONSTER:
                            break;
                        case BrickType.SUPPLY:

                            Debug.Log("玩家点击了补给！");

                            if (Player.standBrick.pathNode.Distance(brick1.pathNode) == 1)
                            {
                                var item = brick1.item as Supply;

                                item.Reactive();
                            }
                            break;
                    }
                }

            }

            Debug.Log("关卡action1： 玩家执行完毕");

            //等待玩家动作结束信号
            yield return playerActionFinish;

            Debug.Log("根据消耗时间移动地图");

            totalRound += 1;
        }
    }

    IEnumerator MovePlayer()
    {
        if (StageCore.Instance.Player.moveComponent.IsNextCanMove())
        {
            yield return StageCore.Instance.Player.moveComponent.MoveToNext(0.3f);

            Debug.Log("Move GO!");

            isPlayerActionFilish = true;

            if (StageCore.Instance.Player.moveComponent.PathFinish)
            {

                //移动结束，清除路径
                StageView.Instance.CancelPahtNode();
                //取消自动loop
                autoMove = false;
            }
            else
            {
                //设为自动状态 下个loop逻辑会自动进行
                autoMove = true;
            }
        }
        else
        {
            Debug.Log("触发下一个砖块效果！");

            //TODO: 触发非空砖块

            autoMove = false;
            isPlayerActionFilish = true;
            StageView.Instance.CancelPahtNode();
        }
    }

    public IEnumerator StopLoop()
    {
        //停止逻辑循环
        CoroCore.Instance.StopCoro("RunLoop");
        //重置标志
        isLooping = false;

        yield return 0;
    }

    public void AddTurnTime(float time)
    {
        turnTime += time;

        Messenger<float>.Invoke(StageAction.StageTimeCast, time);
    }

    public void MoveMap(float time)
    {

    }
}

public static class StageAction
{
    public const string PlayerClickBrick = "PCB";
    public const string RefreshGameItemPos = "RGIP";
    public const string StageTimeCast = "STC";
}
