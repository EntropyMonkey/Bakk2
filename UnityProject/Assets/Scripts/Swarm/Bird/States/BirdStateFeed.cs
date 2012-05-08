using UnityEngine;
using System;

/// <summary>
/// This state manages feeding
/// </summary>
public class BirdStateFeed : IBirdState
{
    private float feedStateTimer;
    public float FeedStateTimer { get { return feedStateTimer; } }

    public new BirdMovementSettings movementSettings = new BirdMovementSettings
    {
        cohesionMultiplier = 1.0f,
        separatingMultiplier = 2.0f,
        targetMultiplier = 10.0f,
        aligningMultiplier = 1.0f,
    };

    private Bird owner;

    public override void Enter(Bird owner)
    {
        this.owner = owner;

        standardColor = owner.standardColorFeeding;


        if (owner.Information.Count > 0)
        {
            // choose food source and copy its information (could be deleted because of age)
            owner.targetPoint = ChooseFoodSource(owner);
        }

        feedStateTimer = 0;
    }

    /// <summary>
    /// Chooses food source based on distance to owner
    /// </summary>
    /// <returns>The position of the chosen food source</returns>
    private Vector3 ChooseFoodSource(Bird owner)
    {
        Vector3 position = owner.Information[0].foodSourcePosition;
        Vector3 ownerPos = owner.transform.position;

        float minDistance = Vector3.Distance(owner.Information[0].foodSourcePosition, ownerPos);

        foreach (BirdInformation information in owner.Information)
        {
            float currentDistance = Vector3.Distance(information.foodSourcePosition, ownerPos);
            if (currentDistance < minDistance)
            {
                position = information.foodSourcePosition;
                minDistance = currentDistance;
            }
        }

        return position;
    }

    public override void Execute(Bird owner)
    {
        feedStateTimer += Time.deltaTime;

        if (owner.Information.Count == 0 || feedStateTimer >= Bird.settings.feed_exploreAfter)
        {
            owner.ChangeState(owner.StateExplore);
        }

        // delete information if it is wrong
        if (Vector3.Distance(owner.transform.position, owner.targetPoint) <= 
            Bird.settings.feed_discreditInfoDistance)
        {
            if (owner.Hungry)
            {
                owner.DiscreditFoodAt(owner.targetPoint);

                if (owner.Information.Count > 0)
                {
                    owner.targetPoint = ChooseFoodSource(owner);
                }
                else
                {
                    owner.ChangeState(owner.StateExplore);
                }
            }
            else
            {
                if (!owner.ignoreBirds && owner.Informed)
                {
                    owner.ChangeState(owner.StateCommunicate);
                }
                else
                {
                    owner.ChangeState(owner.StateExplore);
                }
            }
        }
    }

    public override void FoundFood(Vector3 position)
    {
        owner.targetPoint = position;
    }

    public override void FoundBird(Transform birdTransform)
    {

    }

    public override void Exit(Bird owner)
    {
        owner.HighlightBird(standardColor);
    }
}
