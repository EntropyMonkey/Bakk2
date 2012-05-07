// IPropagationSettings.cs

using UnityEngine;
using System.Collections;

/// <summary>
/// The settings for information propagation between agents. Each agent has own settings.
/// </summary>
public interface IPropagationSettings 
{
    /// <summary>
    /// The certainty a piece of information must have to be remembered
    /// </summary>
    float certaintyThreshold { get; set; }
}
