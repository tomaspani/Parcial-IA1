using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    FStateMachine _fsm;
    [SerializeField] float _speed = 3;

    EPathFinding _pf = new EPathFinding();
    List<Vector3> _path = new List<Vector3>();

    [SerializeField] Nodo _start; 
    [SerializeField] Nodo _end; 

    void Start()
    {
        _fsm = new FStateMachine();

        _fsm.AddState(EnemyStates.Patrol, new EnemyPatrol());
        _fsm.AddState(EnemyStates.Chase, new EnemyChase());

        _fsm.ChangeState(EnemyStates.Patrol);
    }

    void Update()
    {
        _fsm.Update();
    }


    void Patrol()
    {

    }

}


public enum EnemyStates
{
    Patrol,
    Chase
}