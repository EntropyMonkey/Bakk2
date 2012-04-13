// BirdPropagationSettings.cs

using UnityEngine;
using System.Collections;

/// <summary>
/// Determines how a bird propagates information. Each bird has its own propagation settings.
/// </summary>
public class BirdPropagationSettings : IPropagationSettings {
    public float Timeout { set; get; }
}
