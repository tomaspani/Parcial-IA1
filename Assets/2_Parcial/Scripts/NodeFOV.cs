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
        //Debug.DrawLine(transform.position, endPos);
        if (dir.magnitude > _viewRange) return false;
        if (Vector3.Angle(transform.forward, dir) > _viewAngle / 2) return false;
        if (!InLOS(dir))
        {
            Debug.LogError(false);
            return false;
        }
        Debug.LogWarning(true);   
        return true;
    }

    //Line of sight
    bool InLOS(Vector3 dir)
    {
        return !Physics.Raycast(transform.position, dir, dir.magnitude, _wallLayer);
    }





    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, _viewRange);

        Vector3 dirA = GetAngleFromDir(_viewAngle / 2 + transform.eulerAngles.y);
        Vector3 dirB = GetAngleFromDir(-_viewAngle / 2 + transform.eulerAngles.y);
        Gizmos.color = Color.red;

        Gizmos.DrawLine(transform.position, transform.position + dirA.normalized * _viewRange);
        Gizmos.DrawLine(transform.position, transform.position + dirB.normalized * _viewRange);
    }

    Vector3 GetAngleFromDir(float angleInDegrees)
    {
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(Mathf.Deg2Rad * angleInDegrees));
    }
}
