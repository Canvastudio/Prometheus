using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoadData : IState
{

    public string name
    {
        get
        {
            return Predefine.GAME_LOADDATA;
        }
    }

    public IEnumerator DoState()
    {
        yield return SuperTimer.Instance.CoroutineStart(SuperConfig.Instance.LoadAsync(), this);
    }

    public IState GetNextState()
    {
        return GameStateMachine.Instance.GetStateByName(Predefine.GAME_INIT);
    }
}
