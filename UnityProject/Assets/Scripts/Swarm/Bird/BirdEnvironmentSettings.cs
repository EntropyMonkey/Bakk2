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

    /// <summary>
    /// The time between measuring the current status of the system
    /// </summary>
    public float measureTimeout { get; set; }
    /// <summary>
    /// How long the measuring should run
    /// </summary>
    public float maxMeasureTime { get; set; }

    public override string ToString()
    {
        string s = "";
        s += "BirdEnvironmentSettings: \n" +
            "maxDistributedAmountOfFood\t" + maxDistributedAmountOfFood + "\n" +
            "distributeFoodAfterSeconds\t" + distributeFoodAfterSeconds + "\n" +
            "maxBirds\t" + maxBirds + "\n" +
            "bounds\t" + bounds + "\n" +
            "measureTimeout\t" + measureTimeout + "\n" +
            "maxMeasureTime\t" + maxMeasureTime + "\n" + "\n";
        return s;
    }
}
