using UnityEngine;
using System.Collections;

/// <summary>
/// An obstacle holds information which birds can obtain.
/// </summary>
public class Food : MonoBehaviour, IOfferInformation<BirdInformation>
{
    public static int nextFreeId = 0;
    public int id;

    public static int discoveredFood = 0;
    private float discoveredTimestamp = 0;
    public bool discovered
    {
        get
        {
            return discoveredTimestamp > 0;
        }
    }
    public float discoverTimestamp { get { return discoveredTimestamp; } }

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
            amount = value; // debugging
            Information.foodSourceSize = value;
            Vector3 scale = Vector3.zero;
            scale.x = scale.y = scale.z = settings.maxScale * amountOfFood / settings.maxAmountOfFood;
            transform.localScale = scale;
        }
    }

    public float amount;

    private float timeToLive;
    private float spawnTimestamp;
    public float timeAlife
    {
        get
        {
            return Time.time - spawnTimestamp;
        }
    }

    // color/highlighting
    private Color currentColor;
    private Color standardColor;
    public Color eatingHighlightColor;
    private float currentBlendingTime;
    public float blendingTime;

    private void Awake()
    {
        gameObject.tag = GlobalNames.Tags.IOfferInformation;

        id = nextFreeId++;

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
            foodId = id,
        };

        standardColor = currentColor = renderer.material.color;
    }

    private void Start()
    {
    }

    private void OnEnable()
    {
        // position is reset before start is called
        Information.foodSourcePosition = transform.position;

        currentBlendingTime = 0;

        // set timer
        spawnTimestamp = Time.time;
        timeToLive = Random.Range(settings.minTimeAlive, settings.maxTimeAlive);
        discoveredTimestamp = 0;
    }

    private void Update()
    {
        timeToLive -= Time.deltaTime;
        if (timeToLive <= 0.0f)
        {
            Environment.RemoveFood(this);
            standardColor = Color.green;
            if (discovered)
            {
                discoveredFood--;
            }
        }

        // update color
        currentBlendingTime += Time.deltaTime;
        renderer.material.color = Color.Lerp(currentColor, standardColor, currentBlendingTime / blendingTime);
    }
    
    private void HighlightFood(Color highlightColor)
    {
        currentColor = highlightColor;
        currentBlendingTime = 0;
    }

    /// <summary>
    /// Called from the bird's interactive trigger
    /// </summary>
    /// <param name="requestedAmount">the amount of food the bird wants to eat</param>
    /// <returns>the amount of food the source can provide</returns>
    public float Eat(float requestedAmount)
    {
        HighlightFood(eatingHighlightColor);

        // if called with zero requested amount, the food source has been sighted
        if (requestedAmount == 0)
        {
            standardColor = Color.magenta;

            if (!discovered)
            {
                discoveredTimestamp = Time.time;
                discoveredFood++;
            }

            return 0;
        }

        float realAmount = requestedAmount;

        // is there enough food
        if (AmountOfFood - requestedAmount < 0.0f)
        {
            realAmount = AmountOfFood;
        }

        AmountOfFood -= realAmount;
        Environment.RemoveFood(realAmount);

        // remove food
        if (AmountOfFood <= settings.minAmountOfFood)
        {
            Environment.RemoveFood(this);
            if (discovered)
            {
                discoveredFood--;
            }
            standardColor = Color.green;
        }

        return realAmount;
    }
}
