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
    public Transform BirdPrefab;
    public bool ignoreBirds;
    public int numberOfBirds;
    public float foodPercentage;

    public static BirdEnvironmentSettings settings;

    // food
    private const int initialFood = 20;
    private List<Food> foodPool;
    private List<Food> distributedFood;
    private float currentDistributedAmountOfFood = 0;
    private float foodTimer = 0;
	
    // birds
    private List<GameObject> birds;
    private float currentAmountOfBirds = 0;

    // measurement
    private float measurementTimer;

    private void Start () 
    {
        // set bird settings
        Bird.settings = new BirdSettings
        {
            maxAge = 60.0f,
            maxHops = 2,
            eatingThreshold = 0.5f,
            saturationDecreaseIn = 240.0f,
            informationThreshold = 0.5f,
            informationNeedSaturationPerInfo = 1.0f,
            maxFoodCapacity = 2.0f,
            timeout = 0.2f,
            ignoreBirds = ignoreBirds,

            maxVelocity = 20.0f,
            minVelocity = 10.0f,

            explore_changeDirectionAfter = 10.0f,
            explore_minStateTime = 1.0f,
            explore_changeCohesionAfter = 2.0f,

            feed_discreditInfoDistance = 2.0f,
            feed_exploreAfter = 10.0f,
        };

        // set environment settings
        settings = new BirdEnvironmentSettings
        {
            maxBirds = numberOfBirds,
            bounds = collider.bounds,
            distributeFoodAfterSeconds = 2.0f,
            measureTimeout = 5.0f,
        };
        settings.maxDistributedAmountOfFood = Bird.settings.maxFoodCapacity * settings.maxBirds * foodPercentage;

        // set food settings
        Food.settings = new FoodSettings
        {
            // a single food source can never have more than 10% of the whole amount of food
            maxAmountOfFood = settings.maxDistributedAmountOfFood * 0.1f, 
            maxScale = 10.0f,
            timeAlive = 60.0f,
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

        birds = new List<GameObject>();

        while (currentAmountOfBirds < settings.maxBirds)
        {
            CreateBird(gameObject.collider.bounds);
        }
	}
 
    private void Update() 
    {
        ManageFood();

        UpdateMeasurements();
	}

    private void OnTriggerEnter(Collider other)
    {
        // if a bird exits the environment, a force is turned on which will steer it back
        Bird bird = other.gameObject.GetComponent<Bird>();
        if (bird)
        {
            bird.outsideEnvironmentMultiplier = 0.0f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // if a bird exits the environment, a force is turned on which will steer it back
        Bird bird = other.gameObject.GetComponent<Bird>();
        if (bird)
        {
            bird.outsideEnvironmentMultiplier = 1.0f;
        }
    }

    #region Measuring
    private void UpdateMeasurements()
    {
        measurementTimer -= Time.deltaTime;
        if (measurementTimer <= 0.0f)
        {
            measurementTimer = settings.measureTimeout;
            Messenger.Invoke(GlobalNames.Events.UpdateMeasurements);
        }
    }
    #endregion

    #region Birds
    private void CreateBird(Bounds bounds)
    {
        Vector3 randomPosition = Vector3.zero;
        // calculate random position
        randomPosition.x =
            Random.Range(bounds.center.x - bounds.extents.x, bounds.center.x + bounds.extents.x);
        randomPosition.y =
            Random.Range(bounds.center.y - bounds.extents.y, bounds.center.y + bounds.extents.y);
        randomPosition.z =
            Random.Range(bounds.center.z - bounds.extents.z, bounds.center.z + bounds.extents.z);

        Transform instantiatedPrefab = (Transform)Instantiate(BirdPrefab, randomPosition, Quaternion.identity);
        instantiatedPrefab.gameObject.active = true;
        birds.Add(instantiatedPrefab.gameObject);
        currentAmountOfBirds++;
    }
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
        Food newFood = ((Transform)instantiatedPrefab).gameObject.GetComponent<Food>();

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
            Random.Range(1, Food.settings.maxAmountOfFood),
            settings.maxDistributedAmountOfFood - currentDistributedAmountOfFood);
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
        distributedFood.Remove(instance);
        foodTimer = 0;
    }

    public void RemoveFood(float amount)
    {
        currentDistributedAmountOfFood -= amount;
    }
    #endregion
}
