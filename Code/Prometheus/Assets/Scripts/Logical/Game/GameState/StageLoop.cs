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

    IEnumerator ie;
    public IEnumerator DoState()
    {
        //开始执行关卡内部的逻辑循环
        ie = StageCore.Instance.RunLoop();
        yield return ie;
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
