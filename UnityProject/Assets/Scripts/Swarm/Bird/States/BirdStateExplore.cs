using UnityEngine;
using System;

/// <summary>
/// This state manages exploring the bird's environment
/// </summary>
public class BirdStateExplore : IBirdState
{
    private float changeDirectionTimer;

    private float cohesionToggleTimer;

    protected new static BirdMovementSettings moveSettings = new BirdMovementSettings
    {
        // set force multipliers
        cohesionMultiplier = -5.0f,
        separatingMultiplier = 1.0f,
        targetMultiplier = 0.1f,
        aligningMultiplier = 1.0f,
    };
    public override BirdMovementSettings movementSettings
    {
        get { return moveSettings; }
    }

    private void OnEnable()
    {
    }

    public override void Enter(Bird owner)
    {
        birdsExploring++;

        standardColor = owner.standardColorIdle;

        moveSettings.cohesionMultiplier = -5.0f;

        //owner.StateGlobal.ChangeDirection();
        changeDirectionTimer = 0;
        cohesionToggleTimer = 0;
    }

    public override void Execute(Bird owner)
    {
        cohesionToggleTimer += Time.deltaTime;
        if (cohesionToggleTimer > Bird.settings.explore_changeCohesionAfter)
        {
            moveSettings.cohesionMultiplier = moveSettings.cohesionMultiplier == 2.0f ? -5.0f : 2.0f;
            cohesionToggleTimer = 0;
        }
        
        // state change
        if (owner.Hungry && owner.Information.Count > 0)
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
    }

    public override void FoundBird(Transform bird)
    {
    }

    public override void Exit(Bird owner)
    {
        // fade out search state color
        owner.HighlightBird(standardColor);
    }
}
