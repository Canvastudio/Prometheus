using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCore : SingleObject<StageCore> {

    Dictionary<ulong, Monster> monsterDic = new Dictionary<ulong, Monster>();
    Player _player;

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
        _player = player;
    }

    public IEnumerator RunLoop()
    {
        isLooping = true;

        while (isLooping)
        {
            isPlayerActionFilish = false;
            Messenger<Brick>.AddListener(StageAction.PlayerClickBrick, HandlerBrickClick);
            yield return playerActionFinish;

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
    /// <summary>
    /// 负责处理玩家点击砖块的逻辑
    /// </summary>
    /// <param name="brick"></param>
    private void HandlerBrickClick(Brick brick)
    {
        //停止监听砖块点击事件
        //Messenger<Brick>.RemoveListener(StageAction.PlayerClickBrick, HandlerBrickClick);

        switch (brick.brickType)
        {
            case BrickType.EMPTY:

                StageView.Instance.CancelPahtNode();

                CoroCore.Instance.StartCoroutine(Pathfinding.PathfindMaster.Instance.RequestPathfind(_player.standBrick.pathNode, brick.pathNode, (List<Pathfinding.Node> list) =>
                {
                    Debug.Log("path find finish");

                    foreach (var node in list)
                    {
                        Debug.Log("node: " + node.behavirour.name);
                    }

                    StageView.Instance.SetNodeAsPath(list);

                }, BrickCore.Instance));
                break;
        }
    }
}

public static class StageAction
{
    public const string PlayerClickBrick = "PlayerClickBrick";
}
