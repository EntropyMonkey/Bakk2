using UnityEngine;
using System.Collections;

public struct BirdSettings
{
    /// <summary>
    /// A maximum number of hops (giving info from bird to bird)
    /// </summary>
    public int maxHops { get; set; }

    /// <summary>
    /// The maximum age of a piece of information before it's forgotten
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
    /// The saturation is fully decreased in this many seconds
    /// </summary>
    public float saturationDecreaseIn { get; set; }

    /// <summary>
    /// When the need for eating reaches this value, the bird looks for information
    /// </summary>
    public float informationThreshold { get; set; }
    /// <summary>
    /// The information need is saturated per information
    /// </summary>
    public float informationNeedSaturationPerInfo { get; set; }
    
    /// <summary>
    /// when the significance of a piece of information reaches this threshold, it's stored
    /// this threshold must have a value equal to or bigger than 1 (see Bird.GatherInformation method)
    /// </summary>
    public float significancyThreshold { get; set; }

    /// <summary>
    /// The length of time after having gathered information in which no information can be gathered
    /// </summary>
    public float timeout { get; set; }

}
