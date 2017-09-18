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
        BrickCore.Instance.CreatePrimitiveStage();

        return null;
    }

    public IState GetNextState()
    {
        return null;
    }
}
