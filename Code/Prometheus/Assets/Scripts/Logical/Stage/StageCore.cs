using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCore : SingleObject<StageCore> {

    Dictionary<ulong, Monster> monsterDic = new Dictionary<ulong, Monster>();

    public Player Player;

    ulong monsterId = 0;
    bool isPlayerActionFilish;
    WaitUntil playerActionFinish;

    public bool isLooping = false;  

    protected override void Init()
    {
        base.Init();

        playerActionFinish = new WaitUntil(() => isPlayerActionFilish);
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

            //开始处理玩家点击砖块的事件
            yield return new Messenger<Brick>.WaitForMsg(StageAction.PlayerClickBrick.ToString(), HandlerBrickClick);

            Debug.Log("关卡action1： 点击事件完毕");

            //等待玩家动作结束信号
            yield return playerActionFinish;
        }
    }

    public IEnumerator HandlerBrickClick(Brick brick)
    {
        //停止监听砖块点击事件
         switch (brick.brickType)
        {
            case BrickType.EMPTY:

                StageView.Instance.CancelPahtNode();

                var list = Pathfinding.PathfindMaster.Instance.RequestPathfind(Player.standBrick.pathNode, brick.pathNode, BrickCore.Instance);

                yield return StageCore.Instance.Player.moveComponent.Go(list);

                Debug.Log("Move GO!");

                break;
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
