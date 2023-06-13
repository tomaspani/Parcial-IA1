using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum PathfindingType
{
    BFS,
    Dijkstra,
    GreedyBFS,
    AStar
}
//Este script es utilizado para poder visualizar pathfinding en este proyecto
//Cuando hagan sus propios proyectos este script no va a ser falta utilizarlo
public class PathFindingManager : MonoBehaviour
{
    public static PathFindingManager instance;

    //Cambiado por codigo
    public Node startingNode;
    public Node goalNode;
    [SerializeField] GridGenerator _grid;

    PathFinding _pf = new PathFinding();

    public PathfindingType pfType;

    Dictionary<PathfindingType, Func<Node, Node, IEnumerator>> _pfRoutines;

    //action = ? = Reciben o no parametros, y devuelven nada
    //func = ? = Si o si devuelven algo, y reciben o no parametros

    //Func<int, bool>

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        _pfRoutines = new Dictionary<PathfindingType, Func<Node, Node, IEnumerator>>();
        _pfRoutines.Add(PathfindingType.BFS, _pf.BFSRoutine);
        _pfRoutines.Add(PathfindingType.Dijkstra, _pf.DijkstraRoutine);
        _pfRoutines.Add(PathfindingType.GreedyBFS, _pf.GreedyBFSRoutine);
        _pfRoutines.Add(PathfindingType.AStar, _pf.AStarRoutine);
    }

    void Update()
    {
        if (startingNode == null && goalNode == null) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _grid?.ResetGridColors(startingNode, goalNode);
            // ExecutePFRotine();
            StartCoroutine(_pfRoutines[pfType](startingNode, goalNode));
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }

    #region Sin Diccionario de delegados
   /* void ExecutePFRotine()
    {
        switch (pfType)
        {
            case PathfindingType.BFS:
                StartCoroutine(_pf.BFSRoutine(startingNode, goalNode));
                break;
            case PathfindingType.Dijkstra:
                StartCoroutine(_pf.DijkstraRoutine(startingNode, goalNode));
                break;
        }
    }*/
    #endregion

    public void SetStartingNode(Node node)
    {
        //if(_startingNode != null)     
        startingNode?.ChangeColor(Color.white);
        startingNode = node;
        node.ChangeColor(Color.red);

    }

    public void SetGoalNode(Node node)
    {
        goalNode?.ChangeColor(Color.white);
        goalNode = node;
        goalNode.ChangeColor(Color.green);
    }


}
