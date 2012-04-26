// BirdEnvironmentSettings.cs

using UnityEngine;
using System.Collections;

/// <summary>
/// Settings for the bird environment
/// </summary>
public struct BirdEnvironmentSettings
{
    /// <summary>
    /// The maximum amount of food existing in the environment
    /// </summary>
    public float maxDistributedAmountOfFood { get; set; }
    /// <summary>
    /// The timeout for distributing food
    /// </summary>
    public float distributeFoodAfterSeconds { get; set; }

    /// <summary>
    /// the maximum number of birds existing in the environment
    /// </summary>
    public int maxBirds { get; set; }

    /// <summary>
    /// The environment's bounds
    /// </summary>
    public Bounds bounds { get; set; }
}
