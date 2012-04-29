// BirdEnvironment.cs

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// An environment for birds.
/// </summary>
public class BirdEnvironment : Environment<BirdInformation>
{
    public Transform FoodPrefab;

    public static BirdEnvironmentSettings settings;

    private const int initialFood = 20;
    private List<Food> foodPool;
    private List<Food> distributedFood;
    private float currentDistributedAmountOfFood = 0;
    private float foodTimer = 0;
	
    private void Start () 
    {
        // set bird settings
        Bird.settings = new BirdSettings
        {
            maxAge = 180.0f,
            maxHops = 10,
            eatingThreshold = 0.5f,
            informationThreshold = 0.5f,
            informationNeedSaturationPerInfo = 0.0f,
            maxFoodCapacity = 2.0f,
        };

        // set environment settings
        settings = new BirdEnvironmentSettings
        {
            maxBirds = 200,
            bounds = collider.bounds,
            distributeFoodAfterSeconds = 2.0f,
        };
        settings.maxDistributedAmountOfFood = Bird.settings.maxFoodCapacity * settings.maxBirds * 0.5f;

        // set food settings
        Food.settings = new FoodSettings
        {
            // a single food source can never have more than 10% of the whole amount of food
            maxAmountOfFood = settings.maxDistributedAmountOfFood * 0.1f, 
            maxScale = 10.0f,
            timeAlive = 6000.0f,
        };

        // set the maximum difference two pieces of information about food can exhibit after having defined
        // the settings
        BirdInformation.UpdateMaxFoodVectorDifference();

        foodPool = new List<Food>(initialFood);
        distributedFood = new List<Food>();

        // create food
        for (int i = 0; i < initialFood; i++)
        {
            CreateFood();
        }
        
        // distribute food
        while (currentDistributedAmountOfFood < settings.maxDistributedAmountOfFood)
        {
            DistributeFood(gameObject.collider.bounds);
        }
	}

    private void Update() 
    {
        ManageFood();
	}

    #region Birds

    #endregion

    #region Food
    private void ManageFood()
    {
        foodTimer -= Time.deltaTime;

        if (foodTimer <= 0.0f && currentDistributedAmountOfFood < settings.maxDistributedAmountOfFood)
        {
            foodTimer = settings.distributeFoodAfterSeconds;
            DistributeFood(gameObject.collider.bounds);
        }
    }

    /// <summary>
    /// Creating food for the pool
    /// </summary>
    private Food CreateFood()
    {
        Object instantiatedPrefab = Instantiate(FoodPrefab, Vector3.zero, Quaternion.identity);
        Food newFood = (instantiatedPrefab as Transform).gameObject.GetComponent<Food>();

        newFood.gameObject.active = false;
        newFood.Environment = this;
        
        foodPool.Add(newFood);

        return newFood;
    }

    /// <summary>
    /// Distributing food while taking into account already existing amount of food
    /// </summary>
    /// <param name="bounds"></param>
    private void DistributeFood(Bounds bounds)
    {
        Vector3 randomPosition = Vector3.zero;
        // calculate random position
        randomPosition.x = 
            Random.Range(bounds.center.x - bounds.extents.x, bounds.center.x + bounds.extents.x);
        randomPosition.y = 
            Random.Range(bounds.center.y - bounds.extents.y, bounds.center.y + bounds.extents.y);
        randomPosition.z = 
            Random.Range(bounds.center.z - bounds.extents.z, bounds.center.z + bounds.extents.z);

        // get a food instance
        Food freeFood = foodPool.Find(item => !item.gameObject.active);

        // create an instance if there is none free
        if (freeFood == null)
        {
            freeFood = CreateFood();
        }

        // reset new free food
        freeFood.transform.position = randomPosition;
        freeFood.AmountOfFood = Mathf.Min(
            Random.Range(1, settings.maxDistributedAmountOfFood - currentDistributedAmountOfFood),
            Food.settings.maxAmountOfFood);
        // set active after initializing
        freeFood.gameObject.active = true;

        currentDistributedAmountOfFood += freeFood.AmountOfFood;
        distributedFood.Add(freeFood);
    }

    /// <summary>
    /// Called from the food instance when it's food has been eaten
    /// </summary>
    /// <param name="instance">the instance calling the method</param>
    public void RemoveFood(Food instance)
    {
        instance.gameObject.active = false;
        currentDistributedAmountOfFood -= instance.AmountOfFood;
        distributedFood.Remove(instance);
    }
    #endregion
}
