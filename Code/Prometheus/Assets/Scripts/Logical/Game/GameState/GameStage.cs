using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        yield return CoroCore.Instance.StartInnerCoro(StageCore.Instance.RunLoop());
    }

    public IState GetNextState()
    {
        return null;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
