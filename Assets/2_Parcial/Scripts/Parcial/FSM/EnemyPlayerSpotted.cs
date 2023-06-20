using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayerSpotted : CurrentState
{
    Enemy _enemy;
    Player _player;


    public EnemyPlayerSpotted(Enemy e, Player p)
    {
        _enemy = e;
        _player = p;
    }

    public override void OnEnter()
    {
        _enemy.MakePath(_enemy.transform.position, _player.firstSeenPos);
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
        else if(_enemy.CheckPath())
        {
            _enemy.TravelPath();
        }
        else
        {
            fsm.ChangeState(EnemyStates.Patrol);
        }
    }

}
