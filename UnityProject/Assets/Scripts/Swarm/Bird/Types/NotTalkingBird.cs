using UnityEngine;

public class NotTalkingBird : Bird
{
    protected override void InitializeCommunicationSettings()
    {
        base.InitializeCommunicationSettings();

        communicationSettings.equalityThreshold = 0.1f;
        communicationSettings.certaintyThreshold = 0;
    }
}