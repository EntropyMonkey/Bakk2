// BirdMovementSettings.cs

using UnityEngine;
using System;

/// <summary>
/// a bird's communication settings
/// </summary>
public struct BirdMovementSettings
{
    /// <summary>
    /// The weight used to calculate the cohesion force
    /// </summary>
    public float cohesionMultiplier { get; set; }
    /// <summary>
    /// The weight used to calculate the separating force
    /// </summary>
    public float separatingMultiplier { get; set; }
    /// <summary>
    /// the weight used to calculate the force pointing to the target point
    /// </summary>
    public float targetMultiplier { get; set; }
    /// <summary>
    /// The weight used to calculate alignment
    /// </summary>
    public float aligningMultiplier { get; set; }
}
