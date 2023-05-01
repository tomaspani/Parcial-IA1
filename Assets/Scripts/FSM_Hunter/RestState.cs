using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestState : State
{

    Hunter _hunter;

    public RestState(Hunter h)
    {
        _hunter = h;
    }

    public override void OnEnter()
    {
        _hunter.Rest();
    }

    public override void OnExit()
    {
    }

    public override void Update()
    {
        _hunter.EnergyRegen();
        if(_hunter.FullEnergy)
            fsm.ChangeState(HunterStates.Patrol);

    }
}
