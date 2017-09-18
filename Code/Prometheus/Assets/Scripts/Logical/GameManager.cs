using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private IEnumerator Start()
    {
        SuperTimer.Instance.CreatAndBound(this, 30, true);

        yield return SuperTimer.Instance.CoroutineStart(GameStateMachine.Instance.Begin(Predefine.GAME_LOADDATA), this);
    }
}
