using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : SteeringAgent
{
    [Header("Stats")]
    public float viewRadius;
    public float separationRadius;
    public LayerMask obstacleLayer;

    [Header("Weights")]
    [Range(0f, 10f)] public float separationWeight;
    [Range(0f, 10f)] public float alignmentWeight;
    [Range(0f, 10f)] public float cohesionWeight;
    [Range(0f, 10f)] public float seekingWeight;
    [Range(0f, 10f)] public float avoidanceWeight;
    [Range(0f, 10f)] public float fleeingWeight;

    [SerializeField] Transform _hunter;

    FoodManager _FM;
    SpriteRenderer _spriteRenderer;

    private void Start()
    {
        Vector3 randomDir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        AddForce(randomDir.normalized * _maxSpeed);

        BoidManager.instance.AddBoid(this);

        _FM = FindObjectOfType<FoodManager>();
        _spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        transform.position = BoidManager.instance.UpdateBoundPosition(transform.position);

        CheckHunterRange();
        Flocking();
        Move();
        Arrive(CheckFoodRange());
        
        Vector3 obstacleForce = ObstacleAvoidance();
        //Vector3 force = obstacleForce == Vector3.zero ? CalculateSteering(transform.right * _maxSpeed) : obstacleForce;
        AddForce(obstacleForce * avoidanceWeight);
    }

    private Food CheckFoodRange()
    {
        foreach (Food t in _FM.allFood)
        {
            float distance = Vector3.Distance(t.transform.position, transform.position);

            if (distance < viewRadius)
            {
                _spriteRenderer.color = Color.red;
                return t;
            }
        }
        _spriteRenderer.color = Color.white;
        return null;
    }

    void CheckHunterRange()
    {
        float distance = Vector3.Distance(_hunter.position, transform.position);

        if (distance < viewRadius)
        {
            _spriteRenderer.color = Color.blue;
            AddForce(Flee(_hunter.position) * fleeingWeight);
        }
        
    }

    void Arrive(Food t)
    {
        /*if (Vector3.Distance(t.transform.position, transform.position) <= viewRadius)
            this._spriteRenderer.color = Color.red;*/
        if(t != null)
            AddForce(Seek(t.transform.position) * seekingWeight);
    }

    void Flocking()
    {
        AddForce(Separation(BoidManager.instance.allBoids, separationRadius) * separationWeight);
        AddForce(Alignment(BoidManager.instance.allBoids) * alignmentWeight);
        AddForce(Cohesion(BoidManager.instance.allBoids) * cohesionWeight);
    }


    Vector3 Separation(HashSet<Boid> boids, float radius)
    {
        Vector3 desired = Vector3.zero;
        foreach (var item in boids)
        {
            if (item == this || Vector3.Distance(item.transform.position, transform.position) > radius) continue;

            desired += item.transform.position - transform.position;
        }
        if (desired == Vector3.zero) return desired;
        desired = -desired.normalized * _maxSpeed;

        return CalculateSteering(desired);
    }

    Vector3 Alignment(HashSet<Boid> boids)
    {
        Vector3 desired = Vector3.zero;
        int count = 0;

        foreach (Boid item in boids)
        {
            if (Vector3.Distance(item.transform.position, transform.position) <= viewRadius)
            {
                desired += item._velocity;
                count++;
            }
        }

        desired /= count;

        return CalculateSteering(desired.normalized * _maxSpeed);
    }

    Vector3 Cohesion(HashSet<Boid> boids)
    {
        Vector3 desiredPos = Vector3.zero;
        int count = 0;
        foreach (var item in boids)
        {
            if (item == this) continue;

            if (Vector3.Distance(item.transform.position, transform.position) <= viewRadius)
            {
                desiredPos += item.transform.position;
                count++;
            }
        }
        if (count == 0) return Vector3.zero;
        desiredPos /= count;
        return Seek(desiredPos);
    }

    public void DestroyThis()
    {
        BoidManager.instance.allBoids.Remove(this);
        Destroy(this.gameObject);

    }

    Vector3 ObstacleAvoidance()
    {
        Vector3 desired = default;

        if (Physics.Raycast(transform.position + transform.up / 2, _velocity, viewRadius, obstacleLayer))
            desired = -transform.up;
        else if (Physics.Raycast(transform.position - transform.up / 2, _velocity, viewRadius, obstacleLayer))
            desired = transform.up;
        else return desired;

        return CalculateSteering(desired.normalized * _maxSpeed);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, viewRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, separationRadius);


        Vector3 origin1 = transform.position + transform.up/2;
        Vector3 origin2 = transform.position - transform.up/2;
        Gizmos.DrawLine(origin1, origin1 + transform.right * viewRadius);
        Gizmos.DrawLine(origin2, origin2 + transform.right * viewRadius);
    }
}
