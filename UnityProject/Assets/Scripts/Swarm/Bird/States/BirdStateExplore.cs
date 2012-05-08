using UnityEngine;
using System;

/// <summary>
/// This state manages exploring the bird's environment
/// </summary>
public class BirdStateExplore : IBirdState
{
    private Bird owner;

    private float changeDirectionTimer;

    private float changeStateTimer;

    private float cohesionToggleTimer;

    public new BirdMovementSettings movementSettings = new BirdMovementSettings
    {
        // set force multipliers
        cohesionMultiplier = 1.0f,
        separatingMultiplier = 2.0f,
        targetMultiplier = 0.5f,
        aligningMultiplier = 1.0f,

    };
    public override void Enter(Bird owner)
    {
        standardColor = owner.standardColorIdle;
        this.owner = owner;

        //owner.StateGlobal.ChangeDirection();
        changeDirectionTimer = 0;
        changeStateTimer = 0;
        cohesionToggleTimer = 0;
    }

    public override void Execute(Bird owner)
    {
        cohesionToggleTimer += Time.deltaTime;
        if (cohesionToggleTimer > Bird.settings.explore_changeCohesionAfter)
        {
            movementSettings.cohesionMultiplier = 0.2f;
        }

        changeStateTimer += Time.deltaTime;
        // state change
        if (changeStateTimer >= Bird.settings.explore_minStateTime &&
            owner.Hungry && owner.Information.Count > 0)
        {
            owner.ChangeState(owner.StateFeed);
        }

        changeDirectionTimer += Time.deltaTime;
        if (changeDirectionTimer >= Bird.settings.explore_changeDirectionAfter)
        {
            owner.StateGlobal.ChangeDirection();
            changeDirectionTimer = 0;
        }
    }

    public void FoundFood(Bird owner)
    {
        // hungry -> feed
        if (owner.Hungry)
        {
            owner.ChangeState(owner.StateFeed);
        }
        // not hungry but informed -> communicate
        else if (!owner.ignoreBirds && owner.Informed)
        {
            owner.ChangeState(owner.StateCommunicate);
        }
    }

    public override void FoundBird(Transform bird)
    {
        // talk
        if (!owner.ignoreBirds)
        {
            owner.ChangeState(owner.StateCommunicate);
        }
    }

    public override void Exit(Bird owner)
    {
        // fade out search state color
        owner.HighlightBird(standardColor);
    }
}
