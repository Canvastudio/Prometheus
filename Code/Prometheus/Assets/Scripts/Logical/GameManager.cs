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
        //加载数据
        //SuperTimer.Instance.CoroutineStart(GameStateMachine.Instance.SwitchGameState(GameState.LOAD_DATA), this);
       
    }
}
