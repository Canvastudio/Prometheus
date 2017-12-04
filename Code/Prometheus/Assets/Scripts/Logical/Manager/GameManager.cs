using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingleGameObject<GameManager> {

    public Camera UiCamera;

    protected override void Init()
    {
        base.Init();

        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        SuperTimer.Instance.CreatAndBound(this, 30, true);

        CoroCore.Instance.ExStartCoroutine(GameStateMachine.Instance.Begin(Predefine.GAME_INIT));
    }
}
