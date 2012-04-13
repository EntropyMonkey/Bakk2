// IInformation.cs

using UnityEngine;
using System.Collections;

/// <summary>
/// Each kind of information saves when it has been gathered.
/// </summary>
public interface IInformation {
    float Timestamp { get; set; }
}
