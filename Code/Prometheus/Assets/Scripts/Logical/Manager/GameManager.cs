using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingleGameObject<GameManager> {

    public Camera GCamera;

    protected override void Init()
    {
        base.Init();

        int w = Screen.width;
        int h = Screen.height;
        float size = h * 750f / (w * 200f);

        Instance.GCamera.orthographicSize = size;

        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        GO();
    }

    private void GO()
    {
        SuperTimer.Instance.CreatAndBound(this, 30, true);

        StartCoroutine(GameStateMachine.Instance.Begin(Predefine.GAME_INIT));
    }
}
