using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float _speed = 3;
    [SerializeField] Player player;

    FStateMachine _fsm;
    EPathFinding _pf = new EPathFinding();

    List<Vector3> _path = new List<Vector3>();

    public EnemyFOV fov;

    public List<Nodo> _patrolRoute;

    public int currentWaypoint;


    void Start()
    {
        fov = GetComponent<EnemyFOV>();
        _fsm = new FStateMachine();
        _fsm.AddState(EnemyStates.Patrol, new EnemyPatrol(this, player));
        _fsm.AddState(EnemyStates.Chase, new EnemyChase(this, player));
        _fsm.AddState(EnemyStates.PlayerSpotted, new EnemyPlayerSpotted(this, player));

        _fsm.ChangeState(EnemyStates.Patrol);
    }

    void Update()
    {
        _fsm.Update();
    }

    public void Patrol()
    {
        Vector3 target = _patrolRoute[currentWaypoint].transform.position - Vector3.forward; 
        Vector3 dir = target - transform.position;
        transform.rotation = Quaternion.LookRotation(dir);
        if (fov.InFOV(target))
        {
            transform.position += dir.normalized * _speed * Time.deltaTime;
            if (Vector3.Distance(target, transform.position) <= 0.05f) currentWaypoint++;

            if (currentWaypoint >= _patrolRoute.Count)
                currentWaypoint = 0;
        }
        else
        {
            if(CheckPath())
                TravelPath();
        }
    }

    public void GoToPlayerFirstSeenPos()
    {
        if (fov.InFOV(player.firstSeenPos))
        {
            Chase(player.firstSeenPos);
            if (Vector3.Distance(player.firstSeenPos, transform.position) <= 0.05f) _path.Clear();
        }
        else
        {
            TravelPath();
        }
    }

    public void Chase(Vector3 t)
    {
        Vector3 target = t - Vector3.forward;
        Vector3 dir = target - transform.position;
        transform.rotation = Quaternion.LookRotation(dir);
        transform.position += dir.normalized * _speed * Time.deltaTime;
        
    }

    public void MakePath(Vector3 start, Vector3 goal)
    {
        _path = _pf.AStar(FindNearestNodo(start), FindNearestNodo(goal));
        _path.Reverse();
    }

    public void TravelPath()
    {
        Vector3 target = _path[0] - Vector3.forward;
        Vector3 dir = target - transform.position;
        transform.rotation = Quaternion.LookRotation(dir);
        transform.position += dir.normalized * _speed * Time.deltaTime;

        if (Vector3.Distance(target, transform.position) <= 0.1f) _path.RemoveAt(0);
    }

    public bool CheckPath()
    {
        if (_path.Count > 0)
            return true;
        return false;

    } 

    private Nodo FindNearestNodo(Vector3 pos)
    {
        Collider[] colliders = Physics.OverlapSphere(pos, 15f, fov._nodeLayer);
        float minDistance = 1000f;
        Nodo closestNodo = null;
        foreach (Collider c in colliders)
        {
            var distance = Vector3.Distance(pos, c.transform.position);
            if (distance < minDistance && InLOS(pos, c.transform.position, fov._wallLayer))
            {
                minDistance = distance;
                closestNodo = c.GetComponent<Nodo>();
            }
        }
        return closestNodo;
    }

    bool InLOS(Vector3 start, Vector3 end, LayerMask obstacle)
    {
        Vector3 dir = end - start;
        return !Physics.Raycast(start, dir, dir.magnitude, obstacle);
    }
}


public enum EnemyStates
{
    Patrol,
    Chase,
    PlayerSpotted
}