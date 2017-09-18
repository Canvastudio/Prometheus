using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 简单化的状态机
/// </summary>
public class StateMachineBase<T> : SingleObject<T> where T : new()
{
    protected Dictionary<string, IState> _stateDic = new Dictionary<string, IState>();

    private IState _gameState;

    protected virtual void Register(IState state)
    {
        _stateDic[state.name] = state;
    }

    /// <summary>
    /// 直接跳转到指定游戏状态
    /// </summary>
    /// <param name="nextState"></param>
    /// <returns></returns>
    public IEnumerator SwitchGameState(IState nextState)
    {
        _gameState = nextState;
        yield return SuperTimer.Instance.CoroutineStart(nextState.DoState(), nextState);
    }

    public IEnumerator GetNextState()
    {
        IState next_State = _gameState.GetNextState();
        _gameState = next_State;
        yield return SuperTimer.Instance.CoroutineStart(_gameState.DoState(), _gameState);
    }

    public IState GetStateByName(string name)
    {
        IState _state;

        if (_stateDic.TryGetValue(name, out _state))
        {
            return _state;
        }
        else
        {
            return null;
        }
    }

    public IEnumerator Begin(IState state)
    {
        _gameState = state;

        while (_gameState != null)
        {
            yield return SuperTimer.Instance.CoroutineStart(_gameState.DoState(), _gameState);
            _gameState = _gameState.GetNextState();
        }
    }

    /// <summary>
    /// 从指定的状态开始运行
    /// </summary>
    /// <param name="stateName"></param>
    /// <returns></returns>
    public IEnumerator Begin(string stateName)
    {
        if (_stateDic.TryGetValue(stateName, out _gameState))
        {
            while (_gameState != null)
            {
                yield return SuperTimer.Instance.CoroutineStart(_gameState.DoState(), _gameState);
                _gameState = _gameState.GetNextState();
            }
        }
        else
        {
            throw new System.ArgumentException("错误的stateName");
        }
    }

    protected override void Init()
    {
        base.Init();
    }
}
