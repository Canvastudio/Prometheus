using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageInit : IState {

    public string name
    {
        get
        {
            return Predefine.STAGE_INIT;
        }
    }

    public IEnumerator DoState()
    {
        BrickCore.Instance.CreatePrimitiveStage();

        return null;
    }

    public IState GetNextState()
    {
        throw new NotImplementedException();
    }

    public IEnumerator StopState()
    {
        throw new NotImplementedException();
    }
}
