using UnityEngine;
using System.Collections;

/// <summary>
/// An obstacle holds information which birds can obtain.
/// </summary>
public class Food : MonoBehaviour, IOfferInformation<BirdInformation>
{
    public static FoodSettings settings;

    public BirdInformation Information { get; set; }
    public BirdEnvironment Environment { get; set; }

    private float amountOfFood;
    public float AmountOfFood 
    {
        get
        {
            return amountOfFood;
        }
        set
        {
            amountOfFood = value;
            Information.foodSourceSize = value;
            Vector3 scale = Vector3.zero;
            scale.x = scale.y = scale.z = settings.maxScale * amountOfFood / settings.maxAmountOfFood;
            transform.localScale = scale;
        }
    }

    private float timeToLive;

    private void Awake()
    {
        gameObject.tag = GlobalNames.Tags.IOfferInformation;

        // set timer
        timeToLive = settings.timeAlive;

        // set information to be gathered
        Information = new BirdInformation
        {
            // set when gathering information (only relevant when passing info bird <-> bird)
            firstSeenTimestamp = -1,
            gatheredTimestamp = -1,
            certainty = 1.0f,
            hops = -1,

            // set in environment when disposing the food
            type = BirdInformation.BirdInformationType.FOOD,
            foodSourcePosition = Vector3.zero,
            foodSourceSize = -1,
        };
    }

    private void Start()
    {
        // position is reset before start is called
        Information.foodSourcePosition = transform.position;
    }

    private void Update()
    {
        timeToLive -= Time.deltaTime;
        if (timeToLive <= 0.0f)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
    }

    /// <summary>
    /// Called from the bird's interactive trigger
    /// </summary>
    /// <param name="requestedAmount">the amount of food the bird wants to eat</param>
    /// <returns>the amount of food the source can provide</returns>
    public float Eat(float requestedAmount)
    {
        float realAmount = requestedAmount;

        // is there enough food
        if (AmountOfFood - requestedAmount < 0.0f)
        {
            realAmount = AmountOfFood;
        }

        AmountOfFood -= realAmount;

        // remove eaten food
        if (AmountOfFood <= 0.0f)
        {
            Environment.RemoveFood(this);
        }

        return realAmount;
    }
}
