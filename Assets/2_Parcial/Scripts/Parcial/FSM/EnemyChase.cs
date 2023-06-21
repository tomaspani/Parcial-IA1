using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChase : CurrentState
{
    Enemy _enemy;
    Player _player;

    public EnemyChase(Enemy e, Player p)
    {
        _enemy = e;
        _player = p;
    }


    public override void OnEnter()
    {
        Debug.Log("Chase");
        _player.spotted = true;

    }

    public override void OnExit()
    {

        _player.spotted = false;
    }

    public override void Update()
    {
        if (_enemy.fov.InFOV(_player.transform.position))
            _enemy.Chase(_player.transform.position);
        else
            fsm.ChangeState(EnemyStates.Patrol);
    }
}
