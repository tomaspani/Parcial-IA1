using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PFAgent : MonoBehaviour
{
    PathFinding _pf = new PathFinding();
    List<Vector3> _path = new List<Vector3>();

    [SerializeField] float _speed;

    Node Start { get { return PathFindingManager.instance.startingNode; } }
    Node Goal { get { return PathFindingManager.instance.goalNode; } }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GoToStartingNode();
            //El goal/start para este ejemplo lo conseguimos desde el pathfindingManager
            //cuando hagan sus propios proyectos, estos 2 nodos van a tener que ser calculados
            //proceduralmente por cada uno de los agentes a partir de la necesidad que tengan
            _path = GetPathBasedOnPFType();
            if (_path?.Count > 0) _path.Reverse();
        }

        if (Input.GetMouseButtonDown(0))
        {
            GoToStartingNode();
        }

        if (_path.Count > 0)
        {
            TravelPath();
        }

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

    void GoToStartingNode()
    {
        if (PathFindingManager.instance.startingNode == null) return;
        transform.position = PathFindingManager.instance.startingNode.transform.position - Vector3.forward;
    }

    List<Vector3> GetPathBasedOnPFType()
    {
        switch (PathFindingManager.instance.pfType)
        {
            case PathfindingType.BFS:
                return _pf.BreadthFirstSearch(Start, Goal);
            case PathfindingType.Dijkstra:
                return _pf.Dijkstra(Start, Goal);
            case PathfindingType.GreedyBFS:
                return _pf.GreedyBFS(Start, Goal);
            case PathfindingType.AStar:
                return _pf.AStar(Start, Goal);
        }

        return new List<Vector3>();
    }
}
