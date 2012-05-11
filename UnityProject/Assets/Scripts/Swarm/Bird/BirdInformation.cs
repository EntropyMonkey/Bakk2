// BirdInformation.cs

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// A bird can only handle specific information.
/// </summary>
[System.Serializable]
public class BirdInformation : IInformation
{
    /// <summary>
    /// keeps track of how much information there is with the same id
    /// </summary>
    public static Dictionary<int, int> foodIdCounts;

    /// <summary>
    /// The maximum difference possible when comparing two food type information pieces
    /// This value is linked to circumstances like environment bounds and the maximum amount of food
    /// per food source
    /// </summary>
    public static float maxFoodDifferenceVectorMagnitude = 0;

    /// <summary>
    /// The next free id
    /// </summary>
    public static int nextFreeId = 0;
    /// <summary>
    /// The id of this information
    /// </summary>
    public int id;
    /// <summary>
    /// The id of the food where this info originated - debug purposes
    /// </summary>
    public int foodId;

    /// <summary>
    /// the type of information
    /// </summary>
    public enum BirdInformationType { FOOD }

    /// <summary>
    /// When has this information first been gathered?
    /// </summary>
    public float firstSeenTimestamp { get; set; }
    /// <summary>
    /// When has the current owner gathered this information?
    /// </summary>
    public float gatheredTimestamp { get; set; }
    /// <summary>
    /// The certainty rating of this information. In a range between 0 and 1
    /// </summary>
    public float certainty { get; set; }
    /// <summary>
    /// How many birds have known this information
    /// </summary>
    public int hops { get; set; }
    /// <summary>
    /// How long this information has been around (firstSeenTimestamp)
    /// </summary>
    public float age
    { 
        get 
        {
            if (firstSeenTimestamp != -1)
                return Time.time - firstSeenTimestamp;
            else
                return 0;
        } 
    }
    /// <summary>
    /// The type of information which can be gathered
    /// </summary>
    public BirdInformationType type { get; set; }

    /// <summary>
    /// The size of the food source
    /// </summary>
    public float foodSourceSize { get; set; }
    /// <summary>
    /// The position of the food source
    /// </summary>
    public Vector3 foodSourcePosition { get; set; }

    /// <summary>
    /// Constructor
    /// </summary>
    public BirdInformation()
    {
        id = nextFreeId++;
    }

    static BirdInformation()
    {
        foodIdCounts = new Dictionary<int, int>();
    }

    /// <summary>
    /// needs to be updated each time the environment's bounds or the maximum amount of food per source
    /// changes
    /// </summary>
    public static void UpdateMaxFoodVectorDifference()
    {
        float[] maxFoodDifferenceVector = new float[]
        {
            Bird.settings.maxAge,
            Food.settings.maxAmountOfFood,
            BirdEnvironment.settings.bounds.size.x,
            BirdEnvironment.settings.bounds.size.y,
            BirdEnvironment.settings.bounds.size.z
        };

        for (int i = 0; i < maxFoodDifferenceVector.Length; i++)
        {
            maxFoodDifferenceVectorMagnitude += maxFoodDifferenceVector[i] * maxFoodDifferenceVector[i];
        }

        maxFoodDifferenceVectorMagnitude = Mathf.Sqrt(maxFoodDifferenceVectorMagnitude);
    }

    /// <summary>
    /// Copies the information to a new instance of BirdInformation
    /// </summary>
    /// <returns>returns a new instance of BirdInformation with the same info</returns>
    public BirdInformation Copy()
    {
        // create instance
        BirdInformation newInfo = new BirdInformation();

        // copy values
        newInfo.hops = this.hops;
        newInfo.certainty = this.certainty;
        newInfo.firstSeenTimestamp = this.firstSeenTimestamp;
        newInfo.gatheredTimestamp = -1; // still needs to be set

        newInfo.type = this.type;
        newInfo.foodId = this.foodId;
        newInfo.foodSourcePosition = this.foodSourcePosition;
        newInfo.foodSourceSize = this.foodSourceSize;

        if (!foodIdCounts.ContainsKey(foodId))
        {
            foodIdCounts.Add(foodId, 1);
        }
        else
        {
            foodIdCounts[foodId] += 1;
        }

        return newInfo;
    }

    /// <summary>
    /// Measures the equality of two informations
    /// </summary>
    /// <param name="other">the other information</param>
    /// <returns>a value between 0 and 1. if the value is 0, the two items are exactly the same,
    /// if the value is bigger than zero the two are not the same, if the value has reached 1, the two are
    /// as different as possible</returns>
    public float MeasureEquality(BirdInformation other)
    {
        float value = 1;
        if (other.type == this.type)
        {
            // the type defines which dimensions can be compared
            if (type == BirdInformationType.FOOD)
            {
                // the vector containing the comparable information of this
                float[] infoVectorA = new float[]
                {
                    this.age,
                    this.foodSourceSize,
                    this.foodSourcePosition.x,
                    this.foodSourcePosition.y,
                    this.foodSourcePosition.z
                };

                // the vector containing the comparable information of other
                float[] infoVectorB = new float[]
                {
                    other.age,
                    other.foodSourceSize,
                    other.foodSourcePosition.x,
                    other.foodSourcePosition.y,
                    other.foodSourcePosition.z
                };

                int foodProperties = infoVectorA.Length; // the number of comparable food properties
                float[] infoVectorDistance = new float[foodProperties];
                float sqrMagnitude = 0;
                for (int i = 0; i < foodProperties; i++)
                {
                    infoVectorDistance[i] = infoVectorA[i] - infoVectorB[i];
                    sqrMagnitude += infoVectorDistance[i] * infoVectorDistance[i];
                }
                value = Mathf.Sqrt(sqrMagnitude);

                // to make indifferent to environment circumstances, calculate value in range (0,1)
                value = value / maxFoodDifferenceVectorMagnitude;
            }
        }
        return value;
    }

    public void DebugLogMe()
    {
        Debug.Log(" id " + id +
            "\n firstSeenTimestamp " + firstSeenTimestamp +
            "\n gatheredTimestamp " + gatheredTimestamp +
            "\n hops " + hops +
            "\n age " + age +
            "\n certainty " + certainty +
            "\n type " + type +
            "\n foodSourcePosition " + foodSourcePosition +
            "\n foodSourceSize " + foodSourceSize +
            "\n foodId " + foodId);
    }

    public void OnDestroy()
    {
        foodIdCounts[foodId] = foodIdCounts[foodId] - 1;
        if (foodIdCounts[foodId] == 0)
            foodIdCounts.Remove(foodId);
    }
}
