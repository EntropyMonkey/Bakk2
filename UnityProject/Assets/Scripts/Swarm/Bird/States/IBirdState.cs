using UnityEngine;
using System;

public abstract class IBirdState : FSMState<Bird>
{
    public Color standardColor;

    public BirdMovementSettings movementSettings;

    /// <summary>
    /// Called when the bird has sighted food
    /// </summary>
    public virtual void FoundFood(Vector3 position) { }

    /// <summary>
    /// Called when the bird has sighted another bird
    /// </summary>
    public virtual void FoundBird(Transform bird) { }
}