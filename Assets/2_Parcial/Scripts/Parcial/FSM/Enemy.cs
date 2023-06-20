using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    FStateMachine _fsm;
    [SerializeField] float _speed = 3;
    [SerializeField] Player player;

    EPathFinding _pf = new EPathFinding();
    public EnemyFOV fov;
    List<Vector3> _path = new List<Vector3>();

    [SerializeField] List<Nodo> _patrolRoute;
    int _currentWaypoint;

    [SerializeField] Nodo _start; 
    [SerializeField] Nodo _end; 

    void Start()
    {
        fov = GetComponent<EnemyFOV>();
        _fsm = new FStateMachine();
        _fsm.AddState(EnemyStates.Patrol, new EnemyPatrol(this, player));
        _fsm.AddState(EnemyStates.Chase, new EnemyChase(this, player));
        _fsm.AddState(EnemyStates.PlayerSpotted, new EnemyChase(this, player));

        _fsm.ChangeState(EnemyStates.Patrol);
    }

    void Update()
    {

        _fsm.Update();

    }


    public void Patrol()
    {
        Vector3 target = _patrolRoute[_currentWaypoint].transform.position - Vector3.forward; //Esto lo hacemos en este ejemplo para que no se meta adentro del nodo 
        Vector3 dir = target - transform.position;
        transform.rotation = Quaternion.LookRotation(dir);
        if (fov.InFOV(target))
        {
            transform.position += dir.normalized * _speed * Time.deltaTime;
            if (Vector3.Distance(target, transform.position) <= 0.05f) _currentWaypoint++;

            if (_currentWaypoint >= _patrolRoute.Count)
                _currentWaypoint = 0;
        }
        
        //_path = _pf.AStar(start, goal);
    }

    public Vector3 firstSeenPos;

    public void Chase(Vector3 player)
    {
        Debug.Log(" Chase");
        Vector3 target = player - Vector3.forward; //Esto lo hacemos en este ejemplo para que no se meta adentro del nodo 

        Vector3 dir = target - transform.position;
        transform.rotation = Quaternion.LookRotation(dir);
        transform.position += dir.normalized * _speed * Time.deltaTime;
        
    }


    void TravelPath()
    {

        //Completar la siguiente funcion para que el agente recorra el camino
        //devuelto por pathfinding
        Vector3 target = _path[0] - Vector3.forward; //Esto lo hacemos en este ejemplo para que no se meta adentro del nodo 
        Vector3 dir = target - transform.position;
        transform.position += dir.normalized * _speed * Time.deltaTime;

        if (Vector3.Distance(target, transform.position) <= 0.1f) _path.RemoveAt(0);


    }

}


public enum EnemyStates
{
    Patrol,
    Chase,
    PlayerSpotted
}