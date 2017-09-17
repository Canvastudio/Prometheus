using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateMachine : SingleObject<GameStateMachine> {
    
    public IGameState gameState
    {
        get
        {
            return _gameState;
        }
    }


    private IGameState _gameState;

    protected override void Init()
    {
        
    }

    private void Register(IGameState state)
    {

    }

    public IEnumerator SwitchGameState(IGameState nextState)
    {
        _gameState = nextState;
        nextState.DoState();

        //switch(nextState)
        //{
        //    case GameState.LOAD_DATA:
        //        yield return SuperTimer.Instance.CoroutineStart(SuperConfig.Instance.LoadAsync(), this);
        //        break;
        //    case GameState.INIT:
        //        BrickCore.Instance.CreatePrimitiveStage();
        //        break;
        //}

        yield return 0;
    }

    public IEnumerator GetNextState()
    {
        IGameState next_State = _gameState.GetNextState();
        _gameState = next_State;
        yield return SuperTimer.Instance.CoroutineStart(_gameState.DoState(), _gameState);
    }


}

public interface IGameState
{
    string name
    {
        get;
    }

    IEnumerator DoState();
    IGameState GetNextState();
}