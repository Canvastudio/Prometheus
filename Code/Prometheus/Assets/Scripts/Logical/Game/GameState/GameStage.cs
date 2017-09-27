using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏阶段，首先生成地图和地图上的怪物，其次生成玩家，然后开始跑逻辑循环
/// </summary>
public class GameStage : IState
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
        BrickCore.Instance.CreatePlayer();
        //开始执行关卡内部的逻辑循环
        yield return StageCore.Instance.RunLoop();

        Debug.Log("12312312");
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
