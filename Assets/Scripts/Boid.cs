using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : SteeringAgent
{
    [Header("Stats")]
    public float viewRadius, separationRadius;

    public Transform target;

    //[Header("Weights")]
    //[Range(0f, 3f)] public float separationWeight, alignmentWeight, cohesionWeight;

    private void Start()
    {
        Vector3 randomDir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        AddForce(randomDir.normalized * _maxSpeed);

        BoidManager.instance.AddBoid(this);
    }

    private void Update()
    {
        transform.position = BoidManager.instance.UpdateBoundPosition(transform.position);
        Move();
        Seek(target.position);
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, viewRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, separationRadius);
    }
}
