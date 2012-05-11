using UnityEngine;
using System;

/// <summary>
/// settings for food
/// </summary>
public struct FoodSettings
{
    /// <summary>
    /// The amount of food a source can hold
    /// </summary>
    public float maxAmountOfFood { get; set; }
    public float minAmountOfFood { get; set; }

    /// <summary>
    /// The foodsource having an amount of maxAmountOfFood has this size
    /// </summary>
    public float maxScale { get; set; }

    /// <summary>
    /// How long a foodsource is alive
    /// </summary>
    public float maxTimeAlive { get; set; }
    public float minTimeAlive { get; set; }

    public override string ToString()
    {
        string s = "";
        s += "FoodSettings: \n" +
            "maxAmountOfFood " + maxAmountOfFood + "\n" +
            "minAmountOfFood " + minAmountOfFood + "\n" +
            "maxScale " + maxScale + "\n" +
            "maxTimeAlive " + maxTimeAlive + "\n" +
            "minTimeAlive " + minTimeAlive + "\n" + "\n";
        return s;
    }
}