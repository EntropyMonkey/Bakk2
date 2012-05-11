using UnityEngine;

public class GossipBird : Bird
{
    protected override void InitializeCommunicationSettings()
    {
        base.InitializeCommunicationSettings();

        communicationSettings.equalityThreshold = 0.1f;
        communicationSettings.certaintyThreshold = 0.0001f;
    }
}