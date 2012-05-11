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
    public bool ignoreBirdCommunication;
    public int numberOfBirds;
    public float foodPercentage;

    public static BirdEnvironmentSettings settings;

    // food
    private const int initialFood = 20;
    private List<Food> foodPool;
    private List<Food> distributedFood;
    public float currentDistributedAmountOfFood = 0;
    private int spawnedFoodSinceLastMeasurement = 0;
    private int removedFoodSinceLastMeasurement = 0;
	
    // birds
    private List<Bird> birds;
    private float currentAmountOfBirds = 0;

    // measurement
    public Measurement measurement { get; set; }
    private float measurementTimer;

    private void Start () 
    {
        // set bird settings
        Bird.settings = new BirdSettings
        {
            maxAge = 6000000,
            maxHops = 10,
            foodThreshold = 0.2f,
            saturationDecreaseIn = 240.0f,
            informationThreshold = 0.5f,
            informationNeedSaturationPerInfo = 0.0f,
            maxFoodCapacity = 2.0f,
            timeout = 0.2f,
            ignoreBirdCommunication = ignoreBirdCommunication,

            maxVelocity = 20.0f,
            minVelocity = 10.0f,

            explore_changeDirectionAfter = 10.0f,
            explore_changeCohesionAfter = 5.0f,

            feed_discreditInfoDistance = 0.5f,
            feed_discreditInfoAfterTimeInState = 5.0f,
        };

        // set environment settings
        settings = new BirdEnvironmentSettings
        {
            maxBirds = numberOfBirds,
            bounds = collider.bounds,
            distributeFoodAfterSeconds = 2.0f,
            measureTimeout = 5.0f,
            maxMeasureTime = 840.0f,
        };
        settings.maxDistributedAmountOfFood = Bird.settings.maxFoodCapacity * settings.maxBirds * foodPercentage;

        // set food settings
        Food.settings = new FoodSettings
        {
            // a single food source can never have more than 10% of the whole amount of food
            maxAmountOfFood = settings.maxDistributedAmountOfFood * 0.1f,
            minAmountOfFood = settings.maxDistributedAmountOfFood * 0.1f * 0.5f,
            maxScale = 10.0f * foodPercentage,
            maxTimeAlive = 60.0f,
            minTimeAlive = 60.0f,
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

        // create birds
        birds = new List<Bird>();

        while (currentAmountOfBirds < settings.maxBirds)
        {
            CreateBird(gameObject.collider.bounds);
        }

        // set measurement data
        measurement = GetComponent<Measurement>();
        measurement.environmentSettings = settings;
        measurement.birdSettings = Bird.settings;
        measurement.foodSettings = Food.settings;
        measurementTimer = 0;
	}
 
    private void Update() 
    {
        ManageFood();

        measurementTimer += Time.deltaTime;
        if (measurementTimer > settings.measureTimeout)
        {
            if (Time.time > settings.maxMeasureTime)
            {
                Debug.Break();
                measurement.SaveData();
            }

            UpdateMeasurements();
            measurementTimer = 0;
        }
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
        // create new measure data
        MeasureData data = new MeasureData
        {
            timestamp = Time.time,
        };

        // count hungry birds
        foreach (Bird bird in birds)
        {
            if (bird.WasHungry)
            {
                data.hungryBirds++;
                bird.WasHungry = false;
            }
        }

        // set average food life time and discovered food count
        float averageLifeTime = 0;
        foreach (Food food in distributedFood)
        {
            if (food.discovered)
            {
                data.discoveredFood++;
                data.averageDiscoveryDuration += food.discoverTimestamp;
            }

            averageLifeTime += food.timeAlife;
        }
        averageLifeTime /= distributedFood.Count;
        data.averageDiscoveryDuration /= distributedFood.Count;

        // set spawned food and removed food counts
        data.spawnedFood = spawnedFoodSinceLastMeasurement;
        data.removedFood = removedFoodSinceLastMeasurement;
        spawnedFoodSinceLastMeasurement = 0;
        removedFoodSinceLastMeasurement = 0;

        // set bird state counts
        data.feedingBirds = IBirdState.birdsFeeding;
        data.exploringBirds = IBirdState.birdsExploring;
        data.communicatingBirds = IBirdState.birdsCommunicating;

        // set average duplicates
        int duplicates = 0;
        foreach (KeyValuePair<int,int> kvp in BirdInformation.foodIdCounts)
        {
            duplicates += kvp.Value;
        }
        data.averageDuplicatesPerInformation = (float)duplicates / BirdInformation.foodIdCounts.Count;

        // invoke update event
        Messenger.Invoke(GlobalNames.Events.UpdateMeasurements);

        // set data with recently changed information
        data.averageHops = Bird.averageInformationHops;
        Bird.averageInformationHops = 0;

        data.averageInformationAge = Bird.averageInformationAge;
        Bird.averageInformationAge = 0;

        data.averageInformationCertainty = Bird.averageInformationCertainty;
        Bird.averageInformationCertainty = 0;

        measurement.AddData(data);
    }
    #endregion

    #region Birds
    private void CreateBird(Bounds bounds)
    {
        Transform instantiatedPrefab = (Transform)Instantiate(BirdPrefab, bounds.center, Quaternion.identity);
        instantiatedPrefab.gameObject.active = true;
        birds.Add(instantiatedPrefab.gameObject.GetComponent<Bird>());
        currentAmountOfBirds++;
    }
    #endregion

    #region Food
    private void ManageFood()
    {
        while (currentDistributedAmountOfFood < settings.maxDistributedAmountOfFood &&
            settings.maxDistributedAmountOfFood - currentDistributedAmountOfFood > Food.settings.minAmountOfFood)
        {
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

        spawnedFoodSinceLastMeasurement++;
    }

    /// <summary>
    /// Called from the food instance when it's food has been eaten
    /// </summary>
    /// <param name="instance">the instance calling the method</param>
    public void RemoveFood(Food instance)
    {
        instance.gameObject.active = false;
        currentDistributedAmountOfFood -= instance.amount;
        distributedFood.Remove(instance);
    }

    public void RemoveFood(float amount)
    {
        currentDistributedAmountOfFood -= amount;
        removedFoodSinceLastMeasurement++;
    }
    #endregion
}
