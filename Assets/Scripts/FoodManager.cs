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


    float randX, randY;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        if (currentFood < maxFood)
        {
            randX = checkCoordenates(-21, 21);
            randY = checkCoordenates(-11, 11);

            var s = Instantiate(food, new Vector3(randX, randY, 0), Quaternion.identity);
            allFood.Add(s);
            currentFood++;
        }
    }

    public void EatFood(Food food)
    {
        allFood.Remove(food);
        currentFood--;
    }

    float checkCoordenates(float rangeA, float rangeB)
    {
        float a = Random.Range(rangeA, rangeB);

        while (a < 5 && a > -5f)
        {
            a = Random.Range(rangeA, rangeB);
        }

        return a;
    }
}