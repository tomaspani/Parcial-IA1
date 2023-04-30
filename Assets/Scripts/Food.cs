using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    FoodManager _FM;

    private void Start()
    {
        _FM = FindObjectOfType<FoodManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.GetComponent<Boid>())
        {
            Debug.Log("entro");
            _FM.EatFood(this);
            Destroy(this.gameObject);
        }
    } 
}
