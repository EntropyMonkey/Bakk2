// BirdInformation.cs

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// A bird can only handle specific information.
/// </summary>
public class BirdInformation : IInformation 
{
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
    /// How long this information has been around
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
        newInfo.foodSourcePosition = this.foodSourcePosition;
        newInfo.foodSourceSize = this.foodSourceSize;

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
            if (type == BirdInformationType.FOOD)
            {
                int foodProperties = 4; // there are 4 types of comparable food properties

                // create difference vector in (food) property space with values ranging from 0 to 1
                List<float> diffVector = new List<float>(foodProperties);
                diffVector[0] = Mathf.Abs(other.foodSourceSize - this.foodSourceSize) / 
                    Food.settings.maxAmountOfFood;
                // firstly, move origin to (0, 0, 0), then calculate 0 - 1 ranged values
                diffVector[1] = Mathf.Abs(other.foodSourcePosition.x - this.foodSourcePosition.x) / 
                    BirdEnvironment.settings.bounds.size.x;
                diffVector[2] = Mathf.Abs(other.foodSourcePosition.y - this.foodSourcePosition.y) /
                    BirdEnvironment.settings.bounds.size.y;
                diffVector[3] = Mathf.Abs(other.foodSourcePosition.z - this.foodSourcePosition.z) /
                    BirdEnvironment.settings.bounds.size.z;

                // calculate the distance of the two types of information in (food) property space
                float magnitude = 0;
                for (int i = 0; i < foodProperties; ++i)
                {
                    magnitude += diffVector[i] * diffVector[i];
                }
                magnitude = Mathf.Sqrt(magnitude);
                value = magnitude / foodProperties;
            }
        }
        return value;
    }
}
