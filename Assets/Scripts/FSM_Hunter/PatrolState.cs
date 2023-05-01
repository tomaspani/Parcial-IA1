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
        Debug.Log("a");
        if(_hunter.energy > 0 /* && no hay ningun boid en rango*/)
        {

            Debug.Log("aa");
            _hunter.FollowWaypoints();
            _hunter.EnergyDrain();
        }
        else
        {
            fsm.ChangeState(HunterStates.Rest);
        }
    }

    
}
