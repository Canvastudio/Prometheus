using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateMachine : SingleObject<GameStateMachine> {
    
    public enum GameState
    {
        NIL,
        LOADING,
        INIT,
        PLAYER,
        MONSTER,
        MAP,
        END,
        PAUSE,
    }

    public GameState gameState
    {
        get
        {
            return _gameState;
        }
    }

    private GameState _gameState;

    protected override void Init()
    {

    }

    private void SwitchGameState(GameState nextState)
    {

    }
}
