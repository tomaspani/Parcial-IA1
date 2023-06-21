using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeFOV : MonoBehaviour
{
    [SerializeField] LayerMask _wallLayer;
    [SerializeField] LayerMask _nodeLayer;

    [SerializeField] [Range(0, 360)] float _viewAngle;
    [SerializeField] [Range(0, 15)] float _viewRange;

    public List<Nodo> GetNeighbourghs()
    {
        List<Nodo> result = new List<Nodo>();
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _viewRange, _nodeLayer);
        foreach (var collider in hitColliders)
        {
            if(collider.gameObject != this.gameObject)
                if (InFOV(collider.transform.position))
                {
                    result.Add(collider.GetComponent<Nodo>());
                }
        }

        return result;
    }

    bool InFOV(Vector3 endPos)
    {
        Vector3 dir = (endPos - transform.position).normalized;
        if(!InLOS(transform.position, endPos))
            return false;
        return true;
    }

    bool InLOS(Vector3 start, Vector3 end)
    {
        Vector3 dir = end - start;
        return !Physics.Raycast(start, dir, dir.magnitude, _wallLayer);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, _viewRange);

    }
}
