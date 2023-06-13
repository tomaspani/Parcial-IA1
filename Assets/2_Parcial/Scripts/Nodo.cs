using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nodo : MonoBehaviour
{
    [SerializeField] List<Nodo> _vecinos = new List<Nodo>();



    private NodeFOV _nodeFOV;

    private void Awake()
    {
        _nodeFOV = GetComponent<NodeFOV>();

    }

    private void Start()
    {
        _vecinos = _nodeFOV.GetNeighbourghs();
    }
}
