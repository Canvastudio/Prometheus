using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateMachine : StateMachineBase<GameStateMachine> {

    protected override void Init()
    {
        base.Init();

        Register(new GameLoadData());
        Register(new GameInit());
        Register(new GameStage());
    }
}

