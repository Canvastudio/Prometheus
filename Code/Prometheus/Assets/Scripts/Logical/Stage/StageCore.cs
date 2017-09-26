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
        while (true)
        {
            isPlayerActionFilish = false;
            Messenger<Brick>.AddListener(StageAction.PlayerClickBrick, HandlerBrickClick);
            yield return playerActionFinish;
        }
     
    }

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
