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
    /// The length of time after having gathered information in which no information can be gathered
    /// </summary>
    public float timeout { get; set; }

    // movement
    /// <summary>
    /// Maximum velocity
    /// </summary>
    public float maxVelocity { get; set; }
    /// <summary>
    /// Minimum velocity
    /// </summary>
    public float minVelocity { get; set; }

    /// <summary>
    /// StateExplore: changes direction randomly after this number of seconds
    /// </summary>
    public float explore_changeDirectionAfter { get; set; }
    /// <summary>
    /// StateExplore: The minimum time spent in the explore state
    /// </summary>
    public float explore_minStateTime { get; set; }
    /// <summary>
    /// Change the cohesion multiplier after this number of seconds
    /// </summary>
    public float explore_changeCohesionAfter { get; set; }

    /// <summary>
    /// StateFeed: the distance when food should have been eaten. If the bird is still hungry at this
    /// distance to the food source there is no food source and the information is wrong and can be deleted.
    /// </summary>
    public float feed_discreditInfoDistance { get; set; }
    /// <summary>
    /// StateFeed: change to explore state after this amount of time spent in feed state
    /// </summary>
    public float feed_exploreAfter { get; set; }

    /// <summary>
    /// Should other birds be ignored
    /// </summary>
    public bool ignoreBirds { get; set; }

}
