using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    FStateMachine _fsm;
    [SerializeField] float _speed = 3;
    [SerializeField] List<Nodo> _waypoints = new List<Nodo>();


    void Start()
    {
        _fsm = new FStateMachine();

        _fsm.AddState(EnemyStates.Patrol, new EnemyPatrol());
        _fsm.AddState(EnemyStates.Chase, new EnemyChase());

        _fsm.ChangeState(EnemyStates.Patrol);
    }

    // Update is called once per frame
    void Update()
    {
        _fsm.Update();
    }
}

public enum EnemyStates
{
    Patrol,
    Chase
}