using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding
{
    public WaitForSeconds time = new WaitForSeconds(0.01f);

    public List<Vector3> BreadthFirstSearch(Node start, Node goal)
    {
        Queue<Node> frontier = new Queue<Node>();
        frontier.Enqueue(start);

        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        cameFrom.Add(start, null);

        Node current = default;

        while (frontier.Count != 0)
        {
            current = frontier.Dequeue();

            if (current == goal) break; //terminamos de chequear, creamos el camino mas abajo

            foreach (var next in current.GetNeighbors())
            {
                if (next.Blocked) continue;

                if (!cameFrom.ContainsKey(next))
                {
                    frontier.Enqueue(next);
                    cameFrom.Add(next, current);
                }
            }
        }

        List<Vector3> path = new List<Vector3>();
        if (current != goal) return path;

        //Generación de camino
        while (current != start) //si quiero agregar el start lo cambio por != null
        {
            path.Add(current.transform.position);
            current = cameFrom[current];
        }

        //path.Add(current.transform.position) //Opcional: Si queremos agregar el primero
        //path.Reverse(); //Opcional

        return path;
    }

    public List<Vector3> Dijkstra(Node start, Node goal)
    {
        PriorityQueue<Node> frontier = new PriorityQueue<Node>();
        frontier.Enqueue(start, 0);

        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        cameFrom.Add(start, null);

        Dictionary<Node, int> costSoFar = new Dictionary<Node, int>();
        costSoFar.Add(start, 0);

        Node current = default;

        while (frontier.Count != 0)
        {
            current = frontier.Dequeue();

            if (current == goal) break; //terminamos de chequear, creamos el camino mas abajo

            foreach (var next in current.GetNeighbors())
            {
                if (next.Blocked) continue;

                int newCost = costSoFar[current] + next.Cost;

                if (!costSoFar.ContainsKey(next))
                {
                    costSoFar.Add(next, newCost);
                    frontier.Enqueue(next, newCost);
                    cameFrom.Add(next, current);
                }
                else if (newCost < costSoFar[current])
                {
                    frontier.Enqueue(next, newCost);
                    costSoFar[next] = newCost;
                    cameFrom[next] = current;
                }
            }
        }
        List<Vector3> path = new List<Vector3>();
        if (current != goal) return path;

        while (current != start)
        {
            path.Add(current.transform.position);
            current = cameFrom[current];
        }
        //path.Add(current.transform.position) //Opcional: Si queremos agregar el primero
        //path.Reverse(); //Opcional
        return path;
    }

    //BFS golozo
    public List<Vector3> GreedyBFS(Node start, Node goal)
    {
        PriorityQueue<Node> frontier = new PriorityQueue<Node>();
        frontier.Enqueue(start, 0);

        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        cameFrom.Add(start, null);

        Node current = default;

        while (frontier.Count != 0)
        {
            current = frontier.Dequeue();

            if (current == goal) break;

            foreach (var next in current.GetNeighbors())
            {
                if (next.Blocked) continue;

                if (!cameFrom.ContainsKey(next))
                {
                    frontier.Enqueue(next, Heuristic(next.transform.position, goal.transform.position));
                    cameFrom.Add(next, current);
                }
            }
        }

        List<Vector3> path = new List<Vector3>();
        if (current != goal) return path;

        while (current != start)
        {
            path.Add(current.transform.position);
            current = cameFrom[current];
        }
        return path;
    }

    public List<Vector3> AStar(Node start, Node goal)
    {
        PriorityQueue<Node> frontier = new PriorityQueue<Node>();
        frontier.Enqueue(start, 0);

        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        cameFrom.Add(start, null);

        Dictionary<Node, int> costSoFar = new Dictionary<Node, int>();
        costSoFar.Add(start, 0);

        Node current = default;

        while (frontier.Count != 0)
        {
            current = frontier.Dequeue();

            if (current == goal) break; //terminamos de chequear, creamos el camino mas abajo

            foreach (var next in current.GetNeighbors())
            {
                if (next.Blocked) continue;

                int newCost = costSoFar[current] + next.Cost;

                if (!costSoFar.ContainsKey(next))
                {
                    costSoFar.Add(next, newCost);
                    frontier.Enqueue(next, newCost + Heuristic(next.transform.position, goal.transform.position));
                    cameFrom.Add(next, current);
                }
                else if (newCost < costSoFar[current])
                {
                    frontier.Enqueue(next, newCost + Heuristic(next.transform.position, goal.transform.position));
                    costSoFar[next] = newCost;
                    cameFrom[next] = current;
                }
            }
        }
        List<Vector3> path = new List<Vector3>();
        if (current != goal) return path;

        while (current != start)
        {
            path.Add(current.transform.position);
            current = cameFrom[current];
        }
        //path.Add(current.transform.position) //Opcional: Si queremos agregar el primero
        //path.Reverse(); //Opcional
        return path;
    }

    float Heuristic(Vector3 start, Vector3 end)
    {
        return Vector3.Distance(start, end); //Euclidean distance
        //return Mathf.Abs(end.x - start.x) + Mathf.Abs(end.y - start.y); //Manhattan distance
       // return (end - start).sqrMagnitude; //mas liviano menos presicion
    }

    public IEnumerator BFSRoutine(Node start, Node goal)
    {
        Queue<Node> frontier = new Queue<Node>();
        frontier.Enqueue(start);

        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        cameFrom.Add(start, null);


        Node current = default;
        while (frontier.Count != 0)
        {
            current = frontier.Dequeue();
            current.ChangeColor(Color.gray);
            yield return time;

            if (current == goal)
            {
                //List<Node> path = new List<Node>();
                //Generación de camino
                while (current != start) //si quiero agregar el start lo cambio por != null
                {
                    // path.Add(current);
                    current.ChangeColor(Color.green);
                    current = cameFrom[current];
                    yield return time;
                }
                break;
            }

            foreach (var next in current.GetNeighbors())
            {
                if (next.Blocked) continue;

                if (!cameFrom.ContainsKey(next))
                {
                    frontier.Enqueue(next);
                    cameFrom.Add(next, current);

                    next.ChangeColor(Color.blue);
                    yield return time;
                }
            }
        }
    }

    public IEnumerator DijkstraRoutine(Node start, Node goal)
    {
        PriorityQueue<Node> frontier = new PriorityQueue<Node>();
        frontier.Enqueue(start, 0);

        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        cameFrom.Add(start, null);

        Dictionary<Node, int> costSoFar = new Dictionary<Node, int>();
        costSoFar.Add(start, 0);

        Node current = default;

        while (frontier.Count != 0)
        {
            current = frontier.Dequeue();

            if (current == goal)
            {
                while (current != null)
                {
                    current.ChangeColor(Color.green);
                    current = cameFrom[current];
                    yield return time;
                }
                break; //terminamos de chequear, creamos el camino mas abajo
            }

            current.ChangeColor(Color.gray);
            yield return time;


            foreach (var next in current.GetNeighbors())
            {
                if (next.Blocked) continue;

                int newCost = costSoFar[current] + next.Cost;

                if (!costSoFar.ContainsKey(next))
                {
                    costSoFar.Add(next, newCost);
                    frontier.Enqueue(next, newCost);
                    cameFrom.Add(next, current);
                    next.ChangeColor(Color.blue);
                }
                else if (newCost < costSoFar[current])
                {
                    frontier.Enqueue(next, newCost);
                    costSoFar[next] = newCost;
                    cameFrom[next] = current;
                    next.ChangeColor(Color.cyan);
                }
            }
            yield return time;
        }
    }


    public IEnumerator GreedyBFSRoutine(Node start, Node goal)
    {
        PriorityQueue<Node> frontier = new PriorityQueue<Node>();
        frontier.Enqueue(start, 0);

        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        cameFrom.Add(start, null);


        Node current = default;
        while (frontier.Count != 0)
        {
            current = frontier.Dequeue();
            current.ChangeColor(Color.gray);
            yield return time;

            if (current == goal)
            {
                //List<Node> path = new List<Node>();
                //Generación de camino
                while (current != start) //si quiero agregar el start lo cambio por != null
                {
                    // path.Add(current);
                    current.ChangeColor(Color.green);
                    current = cameFrom[current];
                    yield return time;
                }
                break;
            }

            foreach (var next in current.GetNeighbors())
            {
                if (next.Blocked) continue;

                if (!cameFrom.ContainsKey(next))
                {
                    frontier.Enqueue(next, Heuristic(next.transform.position, goal.transform.position));
                    cameFrom.Add(next, current);

                    next.ChangeColor(Color.blue);
                    yield return time;
                }
            }
        }
    }

     public IEnumerator AStarRoutine(Node start, Node goal)
    {
        PriorityQueue<Node> frontier = new PriorityQueue<Node>();
        frontier.Enqueue(start, 0);

        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        cameFrom.Add(start, null);

        Dictionary<Node, int> costSoFar = new Dictionary<Node, int>();
        costSoFar.Add(start, 0);

        Node current = default;

        while (frontier.Count != 0)
        {
            current = frontier.Dequeue();

            if (current == goal)
            {
                while (current != null)
                {
                    current.ChangeColor(Color.green);
                    current = cameFrom[current];
                    yield return time;
                }
                break; //terminamos de chequear, creamos el camino mas abajo
            }

            current.ChangeColor(Color.gray);
            yield return time;


            foreach (var next in current.GetNeighbors())
            {
                if (next.Blocked) continue;

                int newCost = costSoFar[current] + next.Cost;

                if (!costSoFar.ContainsKey(next))
                {
                    costSoFar.Add(next, newCost);
                    frontier.Enqueue(next, newCost + Heuristic(next.transform.position, goal.transform.position));
                    cameFrom.Add(next, current);
                    next.ChangeColor(Color.blue);
                }
                else if (newCost < costSoFar[current])
                {
                    frontier.Enqueue(next, newCost + Heuristic(next.transform.position, goal.transform.position));
                    costSoFar[next] = newCost;
                    cameFrom[next] = current;
                    next.ChangeColor(Color.cyan);
                }
            }
            yield return time;
        }
    }

}
