using UnityEngine;
using System.Collections;

public struct BirdSettings
{
    /// <summary>
    /// A maximum number of hops (giving info from bird to bird)
    /// </summary>
    public int maxHops { get; set; }

    /// <summary>
    /// The maximum age of a piece of information
    /// </summary>
    public float maxAge { get; set; }

    /// <summary>
    /// When the need for eating reaches this value, the bird looks for food and eats
    /// </summary>
    public float eatingThreshold { get; set; }
    /// <summary>
    /// The bird is 100% saturated when it eats this much food
    /// </summary>
    public float maxFoodCapacity { get; set; }

    /// <summary>
    /// When the need for eating reaches this value, the bird looks for information
    /// </summary>
    public float informationThreshold { get; set; }
    
    /// <summary>
    /// when the significance of a piece of information reaches this threshold, it's stored
    /// this threshold must have a value equal to or bigger than 1 (see Bird.GatherInformation method)
    /// </summary>
    private const float significancyThresh = 1.0f;
    public float significancyThreshold { get { return significancyThresh; } }
}
