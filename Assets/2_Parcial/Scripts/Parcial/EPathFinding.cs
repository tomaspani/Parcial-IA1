using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EPathFinding
{
    public WaitForSeconds time = new WaitForSeconds(0.01f);

    public List<Vector3> AStar(Nodo start, Nodo goal)
    {
        PriorityQueue<Nodo> frontier = new PriorityQueue<Nodo>();
        frontier.Enqueue(start, 0);

        Dictionary<Nodo, Nodo> cameFrom = new Dictionary<Nodo, Nodo>();
        cameFrom.Add(start, null);

        Dictionary<Nodo, int> costSoFar = new Dictionary<Nodo, int>();
        costSoFar.Add(start, 0);

        Nodo current = default;

        while (frontier.Count != 0)
        {
            current = frontier.Dequeue();

            if (current == goal) break;

            foreach (var next in current.GetNeighbors())
            {
                //if (next.Blocked) continue;

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
        

        return path;
    }


    float Heuristic(Vector3 start, Vector3 end)
    {
        return Vector3.Distance(start, end); 
                                             
    }
    
}
