using UnityEngine;

public class CertainInfoBird : Bird
{
    protected override void InitializeCommunicationSettings()
    {
        base.InitializeCommunicationSettings();

        communicationSettings.equalityThreshold = 0.5f;
        communicationSettings.certaintyThreshold = 0.9f;
    }
}