using System;
using System.Collections.Generic;

public class BattleStateMachine
{
    private Dictionary<Type, IBattleState> states = new Dictionary<Type, IBattleState>();
    public IBattleState CurrentState { get; private set; }
    public int currentWaveIndex = 1;

    public void AddState<T>(T state) where T : IBattleState
    {
        states[typeof(T)] = state;
    }

    public void ChangeState<T>() where T : IBattleState
    {
        if (CurrentState != null)
        {
            CurrentState.ExitState();
        }

        if (states.TryGetValue(typeof(T), out var newState))
        {
            CurrentState = newState;
            CurrentState.EnterState();
        }
        else
        {
            throw new Exception($"State {typeof(T).Name} not found in State Machine.");
        }
    }

    // 按实例切换状态
    public void ChangeState(IBattleState newState)
    {
        if (CurrentState != null)
        {
            CurrentState.ExitState();
        }

        CurrentState = newState;
        CurrentState.EnterState();
    }

    public void Initialize<T>() where T : IBattleState
    {
        ChangeState<T>();
    }
}
