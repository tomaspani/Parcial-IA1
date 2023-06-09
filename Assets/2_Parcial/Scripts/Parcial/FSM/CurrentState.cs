using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CurrentState
{
    public FStateMachine fsm;

    public abstract void Update();
    public abstract void OnEnter();
    public abstract void OnExit();
}
