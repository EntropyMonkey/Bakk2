using UnityEngine;
using System;

public abstract class IBirdState : FSMState<Bird>
{
    public Color standardColor;

    protected static BirdMovementSettings moveSettings;
    public abstract BirdMovementSettings movementSettings { get; }

    public virtual void UpdateMeasurements()
    {
        birdsCommunicating = 0;
        birdsFeeding = 0;
        birdsExploring = 0;
    }

    /// <summary>
    /// Called when the bird has sighted food
    /// </summary>
    public virtual void FoundFood(Vector3 position) { }

    /// <summary>
    /// Called when the bird has sighted another bird
    /// </summary>
    public virtual void FoundBird(Transform bird) { }

    public static int birdsExploring { get; set; }
    public static int birdsCommunicating { get; set; }
    public static int birdsFeeding { get; set; }

    static IBirdState()
    {
        birdsExploring = 0;
        birdsCommunicating = 0;
        birdsFeeding = 0;
    }

    public IBirdState()
    {
        Messenger.AddListener(GlobalNames.Events.UpdateMeasurements, UpdateMeasurements);
    }
}