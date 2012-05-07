using UnityEngine;
using System;

/// <summary>
/// This state manages feeding
/// </summary>
public class BirdStateFeed : IBirdState
{
    public override void Enter(Bird owner)
    {
        standardColor = owner.standardColorFeeding;

        cohesionMultiplier = 1.0f;
        separatingMultiplier = 1.0f;
        targetMultiplier = 2.0f;
        aligningMultiplier = 0.9f;

        if (owner.Informed)
        {
            // choose food source and copy its information (could be deleted because of age)
            owner.targetPoint = ChooseFoodSource(owner);
        }
        else
        {
            // change state
            owner.ChangeState(owner.StateExplore);
        }
    }

    private Vector3 ChooseFoodSource(Bird owner)
    {
        //TODO
        return owner.Information[0].foodSourcePosition;
    }

    public override void Execute(Bird owner)
    {
        if (!owner.Hungry)
        {
            owner.ChangeState(owner.StateFeed);
        }
        else if (!owner.Informed)
        {
            owner.ChangeState(owner.StateExplore);
        }
    }

    public override void FoundFood(Vector3 position)
    {

    }

    public override void FoundBird(Transform birdTransform)
    {

    }

    public override void Exit(Bird owner)
    {
        owner.HighlightBird(standardColor);
    }
}
