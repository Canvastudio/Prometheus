using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏阶段，首先生成地图和地图上的怪物，其次生成玩家，然后开始跑逻辑循环
/// </summary>
public class StageLoopState : IState
{
    public string name
    {
        get
        {
            return Predefine.GAME_STAGE;
        }
    }

    public IEnumerator DoState()
    {
        //生成地图，怪物
        BrickCore.Instance.CreatePrimitiveStage();

        //生成玩家
        BrickCore.Instance.CreatePlayer(1);

        yield return 0;

        //刷新下位置
        Messenger.Invoke(SA.RefreshGameItemPos);

        //开始执行关卡内部的逻辑循环
        yield return StageCore.Instance.RunLoop();

        Debug.Log("StageLoop 结束！！！！！！！！！！！");
    }

    public IState GetNextState()
    {
  
        return null;
    }

    public IEnumerator StopState()
    {
        yield return StageCore.Instance.StopLoop();
    }
}
