using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : SteeringAgent
{
    [Header("Waypoints")]
    public float waypointRadius;
    [SerializeField] float patrolCost;
    public Transform[] waypoints;


    [Header("Stats")]
    public bool FullEnergy;
    public float energy;
    [SerializeField] float maxEnergy;

    [Header("Chase")]
    [SerializeField] float chaseRadius;
    [SerializeField] float destroyRadius;
    public bool inPursuit;


    public SpriteRenderer sprite;


    private int _currentWaypoint;

    FiniteStateMachine _fsm;
    HashSet<Boid> allBoids;


    private void Start()
    {
        energy = maxEnergy;
        allBoids = BoidManager.instance.allBoids;

        _fsm = new FiniteStateMachine();

        _fsm.AddState(HunterStates.Rest, new RestState(this));
        _fsm.AddState(HunterStates.Patrol, new PatrolState(this));
        _fsm.AddState(HunterStates.Chase, new ChaseState(this));

        _fsm.ChangeState(HunterStates.Patrol);
    }

    private void Update()
    {
        if (energy >= maxEnergy)
            FullEnergy = true;

        if (energy <= 0)
            FullEnergy = false;

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

    public void Chase(SteeringAgent t)
    {
        AddForce(Pursuit(t));
    }

    public SteeringAgent CheckPursuit()
    {
        foreach(Boid b in allBoids)
        {
            if((Vector3.Distance(b.transform.position, transform.position)) <= chaseRadius)
            {
                inPursuit = true;
                return b;
                
            }
        }
        inPursuit = false;
        return null;
    }

    public void DestroyBoid()
    {
        var b = CheckPursuit().GetComponent<Boid>();
        if(Vector3.Distance(b.transform.position, transform.position) <= destroyRadius)
        {
            b.DestroyThis();
        }
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

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, destroyRadius);
    }

}

public enum HunterStates
{
    Rest,
    Patrol,
    Chase
}
