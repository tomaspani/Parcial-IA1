using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{

    Hunter _hunter;

    public ChaseState(Hunter h)
    {
        _hunter = h;
    }

    public override void OnEnter()
    {
        _hunter.sprite.color = Color.red;
    }

    public override void OnExit()
    {

    }

    public override void Update()
    {

        if (_hunter.CheckPursuit() && _hunter.energy > 0 )
        {
            _hunter.Chase(_hunter.CheckPursuit());
            _hunter.EnergyDrain();
            _hunter.DestroyBoid();
        }
        else if (_hunter.CheckPursuit() == null)
        {

            fsm.ChangeState(HunterStates.Patrol);
        }
        else
        {

            fsm.ChangeState(HunterStates.Rest);
        }
    }

}
