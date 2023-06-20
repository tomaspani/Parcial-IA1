using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChase : CurrentState
{
    Enemy enemy;
    Player player;

    public EnemyChase(Enemy e, Player p)
    {
        enemy = e;
        player = p;
    }


    public override void OnEnter()
    {
        player.spotted = true;

    }

    public override void OnExit()
    {
        player.spotted = false;
    }

    public override void Update()
    {
        if (enemy.fov.InFOV(player.transform.position))
            enemy.Chase(player.transform.position);
        else
            fsm.ChangeState(EnemyStates.Patrol);
    }
}
