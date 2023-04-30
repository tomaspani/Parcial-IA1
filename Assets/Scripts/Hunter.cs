using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : SteeringAgent
{
    public Transform[] waypoints;

    public float waypointRadius;

    private int _currentWaypoint;

    private void Update()
    {
        FollowWaypoints();
        Move();
    }

    void FollowWaypoints()
    {
        AddForce(Seek(waypoints[_currentWaypoint].position));

        if (Vector3.Distance(waypoints[_currentWaypoint].position, transform.position) <= waypointRadius)
            _currentWaypoint++;

        if (_currentWaypoint >= waypoints.Length)
            _currentWaypoint = 0;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(waypoints[_currentWaypoint].position, waypointRadius);


    }
}
