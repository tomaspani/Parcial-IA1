using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : CurrentState
{
    Enemy _enemy;
    Player _player;

    public EnemyPatrol(Enemy e, Player p)
    {
        _enemy = e;
        _player = p;
    }

    public override void OnEnter()
    {
        Debug.Log("Patrol");

    }

    public override void OnExit()
    {

    }

    public override void Update()
    {
        var target = _player.transform.position;
        if (_enemy.fov.InFOV(target))
        {

            _player.firstSeenPos = target;
            fsm.ChangeState(EnemyStates.Chase);
        }
        else if (_player.spotted)
        {
            fsm.ChangeState(EnemyStates.PlayerSpotted);
        }
        else
        {
            _enemy.Patrol();
        }
    }
}
