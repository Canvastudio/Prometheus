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
    /// </summary>
    bool autoMove;

    public bool isLooping = false;  

    protected override void Init()
    {
        base.Init();

        playerActionFinish = new WaitUntil(() => isPlayerActionFilish);
        brickMsg = new Messenger<Brick>.WaitForMsg(StageAction.PlayerClickBrick.ToString());
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
                    //如果没有处于自动状态，则等待并处理玩家点击事件
                    yield return brickMsg.BeginWaiting();

                    //事件返回 从事件中得到参数
                    brick1 = brickMsg._para;

                    BrickLogical:
                    //停止监听砖块点击事件
                    switch (brick1.brickType)
                    {
                        case BrickType.EMPTY:

                            StageView.Instance.CancelPahtNode();

                            //开始计算路径
                            var list = Pathfinding.PathfindMaster.Instance.RequestPathfind(Player.standBrick.pathNode, brick1.pathNode, BrickCore.Instance);
                            //标记路径
                            StageView.Instance.SetNodeAsPath(list);
                            //如果路径长度小于3，不需要确认直接移动
                            if (list.Count < 3)
                            {
                                yield return StageCore.Instance.Player.moveComponent.Go(list);
                            }
                            else
                            {
                                //等待玩家点击确认
                                yield return brickMsg.BeginWaiting();

                                brick2 = brickMsg._para;
          
                                if (brick1 == brick2)
                                {
                                    //确认成功
                                    //yield return StageCore.Instance.Player.moveComponent.Go(list);
                                    StageCore.Instance.Player.moveComponent.SetPaht(list);

                                    yield return MovePlayer();
                                }
                                else
                                {
                                    brick1 = brick2;
                                    goto BrickLogical;
                                }
                            }
                            break;
                    }
                }

            }

            Debug.Log("关卡action1： 玩家执行完毕");

            //等待玩家动作结束信号
            yield return playerActionFinish;
        }
    }

    IEnumerator MovePlayer()
    {
        yield return StageCore.Instance.Player.moveComponent.MoveToNext(2f);

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

    public IEnumerator StopLoop()
    {
        //停止逻辑循环
        CoroCore.Instance.StopCoro("RunLoop");
        //重置标志
        isLooping = false;

        yield return 0;
    }
}

public static class StageAction
{
    public const string PlayerClickBrick = "PlayerClickBrick";
}
