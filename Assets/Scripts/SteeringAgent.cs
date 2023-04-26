using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringAgent : MonoBehaviour
{
    protected Vector3 _velocity;
    [SerializeField]
    protected float _maxSpeed;
    [Range(0, 0.1f)]
    [SerializeField]
    protected float _maxForce;


    protected void Move()
    {
        transform.position += _velocity * Time.deltaTime;
        transform.right = _velocity;
    }

    protected void AddForce(Vector3 force)
    {
        _velocity = Vector3.ClampMagnitude(_velocity + force, _maxSpeed);
    }

    protected Vector3 CalculateSteering(Vector3 desired)
    {
        return Vector3.ClampMagnitude(desired - _velocity, _maxForce);
    }

    protected Vector3 Seek(Vector3 target)
    {
        return CalculateSteering((target - transform.position).normalized* _maxSpeed);
    }
}
