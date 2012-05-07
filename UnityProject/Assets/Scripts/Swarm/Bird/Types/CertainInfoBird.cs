using System;
using UnityEngine;

/// <summary>
/// This kind of bird only remembers very certain information and does not need as long as gossipbird to 
/// pass on information because it does not have as much information to talk abou
/// </summary>
public class CertainInfoBird : Bird
{
    /// <summary>
    /// Set the communication settings
    /// </summary>
    private void Start()
    {
        communicationSettings.equalityThreshold = 0.1f;
        communicationSettings.certaintyThreshold = 0.7f;
    }
}
