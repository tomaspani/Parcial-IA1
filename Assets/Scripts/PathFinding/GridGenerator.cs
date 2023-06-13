using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    Node[,] _grid;

    [SerializeField] int _sizeX;
    [SerializeField] int _sizeY;
    [SerializeField] float _offset = 0.1f;
    //[SerializeField] float _scale = 1f;

    [SerializeField] GameObject nodePrefab;

    // Start is called before the first frame update
    void Start()
    {
        // GenerateGrid();
        StartCoroutine(GenerateGridRoutine());
    }

    void GenerateGrid()
    {
        // GameObject[] array = new GameObject[_size];
        _grid = new Node[_sizeX, _sizeY];

        for (int x = 0; x < _sizeX; x++)
        {
            for (int y = 0; y < _sizeY; y++)
            {
                GameObject obj = Instantiate(nodePrefab);
                Node node = obj.GetComponent<Node>();

                obj.transform.position = Vector3.right * (x + (_offset * x)) + Vector3.up * (y + (_offset * y));
                obj.transform.SetParent(transform);

                node.Initialize(this, x, y);
                _grid[x, y] = node;
            }
        }
    }

    public void ResetGridColors(Node start, Node goal)
    {
        for (int x = 0; x < _sizeX; x++)
        {
            for (int y = 0; y < _sizeY; y++)
            {
                Node node = _grid[x, y];
                if (node.Blocked || node == start || node == goal) continue;
                node.ChangeColor(Color.white);
            }
        }
    }

    IEnumerator GenerateGridRoutine()
    {
        _grid = new Node[_sizeX, _sizeY];

        WaitForSeconds time = new WaitForSeconds(0.01f);

        for (int x = 0; x < _sizeX; x++)
        {
            for (int y = 0; y < _sizeY; y++)
            {
                GameObject obj = Instantiate(nodePrefab);
                Node node = obj.GetComponent<Node>();

                obj.transform.position = Vector3.right * (x + (_offset * x)) + Vector3.up * (y + (_offset * y));
                obj.transform.SetParent(transform);

                node.Initialize(this, x, y);
                _grid[x, y] = node;
                yield return time;
            }
        }
    }

    public List<Node> GetNeighborsAtPosition(int x, int y)
    {
        List<Node> neighbors = new List<Node>();
        if (x + 1 < _sizeX) neighbors.Add(_grid[x + 1, y]);
        if (y - 1 >= 0) neighbors.Add(_grid[x, y - 1]);
        if (x - 1 >= 0) neighbors.Add(_grid[x - 1, y]);
        if (y + 1 < _sizeY) neighbors.Add(_grid[x, y + 1]);

        /* if(InGrid(x + 1, y + 1)) neighbors.Add(_grid[x + 1, y + 1]);
         if(InGrid(x - 1, y + 1)) neighbors.Add(_grid[x - 1, y + 1]);
         if(InGrid(x - 1, y - 1)) neighbors.Add(_grid[x - 1, y - 1]);
         if(InGrid(x + 1, y - 1)) neighbors.Add(_grid[x + 1, y - 1]);*/

        return neighbors;
    }

    /*bool InGrid(int x, int y)
       {
           return x >= 0 && y >= 0 && x < _sizeX && y < _sizeY;
       }
    */
}
