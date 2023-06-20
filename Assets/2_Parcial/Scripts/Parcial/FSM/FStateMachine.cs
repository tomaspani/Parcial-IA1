using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FStateMachine
{
    CurrentState _currentState;

    Dictionary<EnemyStates, CurrentState> _allStates = new Dictionary<EnemyStates, CurrentState>();

    public void AddState(EnemyStates key, CurrentState state)
    {
        if (state == null) return;

        _allStates.Add(key, state);
        state.fsm = this;
    }

    public void ChangeState(EnemyStates key)
    {
        if (!_allStates.ContainsKey(key)) return;

        if (_currentState != null)
            _currentState.OnExit();

        _currentState = _allStates[key];
        _currentState.OnEnter();
    }

    public void Update()
    {
        _currentState.Update();
    }

}
