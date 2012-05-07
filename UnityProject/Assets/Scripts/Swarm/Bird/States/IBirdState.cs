using UnityEngine;
using System;

public abstract class IBirdState : FSMState<Bird>
{
    public Color standardColor;

    /// <summary>
    /// The weight used to calculate the cohesion force
    /// </summary>
    public float cohesionMultiplier;
    /// <summary>
    /// The weight used to calculate the separating force
    /// </summary>
    public float separatingMultiplier;
    /// <summary>
    /// the weight used to calculate the force pointing to the target point
    /// </summary>
    public float targetMultiplier;
    /// <summary>
    /// The weight used to calculate alignment
    /// </summary>
    public float aligningMultiplier;

    /// <summary>
    /// Called when the bird has sighted food
    /// </summary>
    public virtual void FoundFood(Vector3 position) { }

    /// <summary>
    /// Called when the bird has sighted another bird
    /// </summary>
    public virtual void FoundBird(Transform bird) { }
}