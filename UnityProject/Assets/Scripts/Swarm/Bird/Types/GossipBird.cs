using System;
using UnityEngine;

/// <summary>
/// This kind of bird does not forget uncertain info, low certainty threshold. It needs a long time to
/// pass on information because it handles more information than the certaininfo bird
/// </summary>
public class GossipBird : Bird
{
    /// <summary>
    /// Set the communication settings
    /// </summary>
    private void Start()
    {
        communicationSettings.equalityThreshold = 0.1f;
        communicationSettings.certaintyThreshold = 0.0f;
    }
}
