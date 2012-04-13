// BirdInformation.cs

using UnityEngine;
using System.Collections;

/// <summary>
/// A bird can only hold specific information.
/// </summary>
public class BirdInformation : IInformation 
{
    public float Timestamp { get; set; }
}
