using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : CurrentState
{
    Enemy enemy;
    Player player;

    public EnemyPatrol(Enemy e, Player p)
    {
        enemy = e;
        player = p;
    }

    public override void OnEnter()
    {

    }

    public override void OnExit()
    {

    }

    public override void Update()
    {
        var target = player.transform.position;
        if (enemy.fov.InFOV(target))
        {
            enemy.firstSeenPos = target;
            fsm.ChangeState(EnemyStates.Chase);
        }
        /*else if (player.spotted)
        {
            //xd
        }*/
        else
        {
            enemy.Patrol();
        }
    }
}
