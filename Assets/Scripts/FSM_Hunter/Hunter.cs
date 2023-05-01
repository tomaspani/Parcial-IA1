using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : SteeringAgent
{
    public Transform[] waypoints;
    public SpriteRenderer sprite;
    public bool FullEnergy;

    [SerializeField] float patrolCost;


    FiniteStateMachine _fsm;

    [SerializeField] float maxEnergy;

    public float energy;
    public float waypointRadius;

    private int _currentWaypoint;

    private void Start()
    {

        energy = maxEnergy;

        _fsm = new FiniteStateMachine();

        _fsm.AddState(HunterStates.Rest, new RestState(this));
        _fsm.AddState(HunterStates.Patrol, new PatrolState(this));


        _fsm.ChangeState(HunterStates.Patrol);

    }

    private void Update()
    {
        if (energy >= maxEnergy)
            FullEnergy = true;

        if (energy <= 0)
            FullEnergy = false;

        //FollowWaypoints();
        Move();
        _fsm.Update();
    }

    public void FollowWaypoints()
    {
        AddForce(Seek(waypoints[_currentWaypoint].position));

        if (Vector3.Distance(waypoints[_currentWaypoint].position, transform.position) <= waypointRadius)
            _currentWaypoint++;

        if (_currentWaypoint >= waypoints.Length)
            _currentWaypoint = 0;
    }

    public void Rest()
    {
        AddForce(-_velocity);
        sprite.color = Color.yellow;
    }

    public void EnergyDrain()
    {
        if (energy > 0)
        {
            energy -= patrolCost * Time.deltaTime;

        }
        else
            energy = 0; 
    }

    public void EnergyRegen()
    {
        if (energy < maxEnergy)
        {
            energy += 5 * Time.deltaTime;

        }
        else 
            energy = maxEnergy;
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(waypoints[_currentWaypoint].position, waypointRadius);

    }

}

public enum HunterStates
{
    Rest,
    Patrol,
    Chase
}
