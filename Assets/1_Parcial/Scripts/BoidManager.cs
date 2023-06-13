using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
    [SerializeField]
    float boundHeight, boundWidth;

    private float _halfWidth { get { return boundWidth / 2; } }
    private float _halfHeigth { get { return boundHeight / 2; } }


    public HashSet<Boid> allBoids = new HashSet<Boid>();

    public static BoidManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void AddBoid(Boid b)
    {
        if (!allBoids.Contains(b))
            allBoids.Add(b);
    }

    public Vector3 UpdateBoundPosition(Vector3 position)
    {
        if (position.x > _halfWidth) position.x = -_halfWidth;
        if (position.x < -_halfWidth) position.x = _halfWidth;
        if (position.y < -_halfHeigth) position.y = _halfHeigth;
        if (position.y > _halfHeigth) position.y = -_halfHeigth;

        return position;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Vector3 topLeft = new Vector3(-_halfWidth, _halfHeigth);
        Vector3 topRight = new Vector3(_halfWidth, _halfHeigth);
        Vector3 bottomRight = new Vector3(_halfWidth, -_halfHeigth);
        Vector3 bottomLeft = new Vector3(-_halfWidth, -_halfHeigth);

        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);
    }
}
