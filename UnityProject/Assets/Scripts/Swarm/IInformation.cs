// IInformation.cs

using UnityEngine;
using System.Collections;

/// <summary>
/// Each kind of information saves when it has been gathered.
/// </summary>
public interface IInformation {
    /// <summary>
    /// When has this information first been gathered?
    /// </summary>
    float firstSeenTimestamp { get; set; }
    /// <summary>
    /// When has the current owner gathered this information?
    /// </summary>
    float gatheredTimestamp { get; set; }
}
