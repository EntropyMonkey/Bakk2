// BirdInformation.cs

using UnityEngine;
using System.Collections;

/// <summary>
/// A bird can only handle specific information.
/// </summary>
public class BirdInformation : IInformation 
{
    public float Timestamp { get; set; }
}
