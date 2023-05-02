using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : State
{

    private Hunter _hunter;

    public PatrolState( Hunter h)
    {
        _hunter = h;
    }

    public override void OnEnter()
    {
        _hunter.sprite.color = Color.blue;
    }

    public override void OnExit()
    {
        _hunter.sprite.color = Color.white;
    }

    public override void Update()
    {
        if(_hunter.energy > 0 && _hunter.CheckPursuit() == null)
        {

            Debug.Log("a");

            _hunter.FollowWaypoints();
            _hunter.EnergyDrain();
        }
        else if(_hunter.CheckPursuit())
        {
            Debug.Log("aaa");
            fsm.ChangeState(HunterStates.Chase);
        }
        else
        {
            fsm.ChangeState(HunterStates.Rest);
        }
    }

    
}
