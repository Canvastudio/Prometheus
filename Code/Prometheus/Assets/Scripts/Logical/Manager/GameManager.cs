using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        SuperTimer.Instance.CreatAndBound(this, 30, true);

        CoroCore.Instance.ExStartCoroutine(GameStateMachine.Instance.Begin(Predefine.GAME_INIT));
    }
}
