using UnityEngine;
using System;

/// <summary>
/// settings for food
/// </summary>
public struct FoodSettings
{
    /// <summary>
    /// The maximum amount of food a source can hold
    /// </summary>
    public float maxAmountOfFood { get; set; }

    /// <summary>
    /// The foodsource having an amount of maxAmountOfFood has this size
    /// </summary>
    public float maxScale { get; set; }

    /// <summary>
    /// How long a foodsource is alive
    /// </summary>
    public float timeAlive { get; set; }
}