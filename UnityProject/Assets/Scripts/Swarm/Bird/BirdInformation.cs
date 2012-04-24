// BirdInformation.cs

using UnityEngine;
using System.Collections;

/// <summary>
/// A bird can only handle specific information.
/// </summary>
public class BirdInformation : IInformation 
{
    /// <summary>
    /// When has this information first been gathered?
    /// </summary>
    public float FirstSeenTimestamp { get; set; }
    /// <summary>
    /// When has the current owner gathered this information?
    /// </summary>
    public float GatheredTimestamp { get; set; }

    /// <summary>
    /// The size of the food source
    /// </summary>
    public float FoodSourceSize { get; set; }
    /// <summary>
    /// The position of the food source
    /// </summary>
    public Vector3 FoodSourcePosition { get; set; }
}
