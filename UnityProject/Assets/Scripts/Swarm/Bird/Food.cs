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
            Vector3 scale = Vector3.zero;
            scale.x = scale.y = scale.z = settings.maxScale * amountOfFood / settings.maxAmountOfFood;
            transform.localScale = scale;
        }
    }

    private float timeToLive;

    private void Start()
    {
        gameObject.tag = GlobalNames.Tags.IOfferInformation;

        // set timer
        timeToLive = settings.timeAlive;

        // set information to be gathered
        Information = new BirdInformation
        {
            firstSeenTimestamp = -1,
            gatheredTimestamp = -1,
            certainty = 1.0f,
            hops = -1,

            type = BirdInformation.BirdInformationType.FOOD,
            foodSourcePosition = transform.position,
            foodSourceSize = -1, // will be set in environment, because it keeps track of total amount of food
        };
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
