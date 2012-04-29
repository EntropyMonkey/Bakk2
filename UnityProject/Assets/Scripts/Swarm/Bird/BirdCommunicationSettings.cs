// BirdCommunicationSettings.cs

using UnityEngine;
using System;

/// <summary>
/// a bird's communication settings
/// </summary>
public struct BirdCommunicationSettings : IPropagationSettings
{
    /// <summary>
    /// How certain a piece of information must be to be seen as certain
    /// </summary>
    public float certaintyThreshold { get; set; }

    /// <summary>
    /// How equal two pieces of information must be to be seen as equal
    /// </summary>
    public float EqualityThreshold { get; set; }

    /// <summary>
    /// The length of time after having gathered information in which no information can be gathered
    /// </summary>
    public float Timeout { get; set; }
}
