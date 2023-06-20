using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CurrentState
{
    public FStateMachine fsm; //comodo pero trae problemas con genericos (tener la referencia en los estados mismos)
                                   //si lo borro me salta error en FStateMachine asi que mejor lo dejo

    public abstract void Update();
    public abstract void OnEnter();
    public abstract void OnExit();
}
