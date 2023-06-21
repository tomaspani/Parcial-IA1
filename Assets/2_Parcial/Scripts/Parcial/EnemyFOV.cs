using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFOV : MonoBehaviour
{

    [SerializeField] [Range(0, 360)] float _viewAngle;
    [SerializeField] [Range(0, 15)] float _viewRange;

    public LayerMask _wallLayer; 
    public LayerMask _nodeLayer;


    public bool InFOV(Vector3 endPos)
    {
        Vector3 dir = endPos - transform.position;
        if (dir.magnitude > _viewRange) return false;
        if (Vector3.Angle(transform.forward, dir) > _viewAngle / 2) return false;
        if (!InLOS(transform.position, endPos)) return false;
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

        Vector3 dirA = GetAngleFromDir(_viewAngle / 2 + transform.eulerAngles.y);
        Vector3 dirB = GetAngleFromDir(-_viewAngle / 2 + transform.eulerAngles.y);
        Gizmos.DrawLine(transform.position, transform.position + dirA.normalized * _viewRange);
        Gizmos.DrawLine(transform.position, transform.position + dirB.normalized * _viewRange);
    }

    Vector3 GetAngleFromDir(float angleInDegrees)
    {
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(Mathf.Deg2Rad * angleInDegrees));
    }

}
