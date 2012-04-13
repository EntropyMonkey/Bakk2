// IPropagationSettings.cs

using UnityEngine;
using System.Collections;

/// <summary>
/// The settings for information propagation between agents. Each agent has own settings.
/// </summary>
public interface IPropagationSettings 
{
    float Timeout { get; set; }
}
