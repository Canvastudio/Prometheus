using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingleGameObject<GameManager> {

    protected override void Init()
    {
        base.Init();

        int w = Screen.width;
        float ch = w * 1334f / 750f / 2 / 100;
        //StageView.Instance.show_camera.orthographicSize = ch;

        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        SuperTimer.Instance.CreatAndBound(this, 30, true);

        CoroCore.Instance.ExStartCoroutine(GameStateMachine.Instance.Begin(Predefine.GAME_INIT));
    }
}
