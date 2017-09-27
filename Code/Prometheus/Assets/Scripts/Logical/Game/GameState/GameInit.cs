using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInit : IState
{
    public string name
    {
        get
        {
            return Predefine.GAME_INIT;
        }
    }

    public IEnumerator DoState()
    {
        yield return CoroCore.Instance.StartCoroutine(SuperConfig.Instance.LoadAsync());
    }

    public IState GetNextState()
    {
        return GameStateMachine.Instance.GetStateByName(Predefine.GAME_STAGE);
    }

    public IEnumerator StopState()
    {
        throw new NotImplementedException();
    }
}
