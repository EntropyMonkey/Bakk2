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
    float FirstSeenTimestamp { get; set; }
    /// <summary>
    /// When has the current owner gathered this information?
    /// </summary>
    float GatheredTimestamp { get; set; }
}
