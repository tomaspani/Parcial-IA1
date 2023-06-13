using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Node : MonoBehaviour
{
    Renderer _renderer;
    List<Node> _neighbors = new List<Node>();
    GridGenerator _grid;
    Coordinates coordinates;

    private bool _blocked;

    int _cost = 1;
    public int Cost { get { return _cost; } }
    [SerializeField] TextMeshProUGUI _costUI;

    public bool Blocked
    {
        get { return _blocked; }
        set { _blocked = value; ChangeColor(value ? Color.black : Color.white); }
    }

    public void Initialize(GridGenerator grid, int x, int y)
    {
        _renderer = GetComponent<Renderer>();
        _grid = grid;
        coordinates = new Coordinates(x, y);
        Blocked = false;
        SetCost(1);
    }

    public List<Node> GetNeighbors()
    {
        if (_neighbors.Count == 0)
        {
            _neighbors = _grid.GetNeighborsAtPosition(coordinates.x, coordinates.y);
        }
        return _neighbors;
    }


    public void ChangeColor(Color color)
    {
        _renderer.material.color = color;
    }

    void OnMouseDown()
    {
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
            PathFindingManager.instance.SetStartingNode(this);
        if (Input.GetMouseButtonDown(1))
            PathFindingManager.instance.SetGoalNode(this);
        if (Input.GetMouseButtonDown(2)) Blocked = !_blocked;

        if (Input.mouseScrollDelta.y > 0) SetCost(_cost + 5);
        if (Input.mouseScrollDelta.y < 0) SetCost(_cost - 5);

    }

    void SetCost(int cost)
    {
        _cost = cost < 1 ? 1 : cost;
        if (cost == 6) _cost = 5;
        ChangeColor(_cost > 1 ? new Color(0, 0.9f, 0) : Color.white);
        _costUI.enabled = _cost != 1;
        _costUI.text = _cost.ToString();
    }


}

struct Coordinates
{
    public Coordinates(int X, int Y)
    {
        x = X;
        y = Y;
    }

    public int x;
    public int y;

}