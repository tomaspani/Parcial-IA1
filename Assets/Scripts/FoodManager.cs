using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodManager : MonoBehaviour
{
    public static FoodManager instance;
    public HashSet<Food> allFood = new HashSet<Food>();
    public Food food;
    public float maxFood = 10;
    public float currentFood;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        if (currentFood < maxFood)
        {
            var s = Instantiate(food, new Vector3(Random.Range(-21f, 21f), Random.Range(-11f, 11f), 0), Quaternion.identity);
            allFood.Add(s);
            currentFood++;
        }
    }

     public void EatFood(Food food)
    {
        allFood.Remove(food);
        currentFood--;
    }
}
