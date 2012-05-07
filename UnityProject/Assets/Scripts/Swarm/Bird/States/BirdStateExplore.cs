using UnityEngine;
using System;

/// <summary>
/// This state manages exploring the bird's environment
/// </summary>
public class BirdStateExplore : IBirdState
{
    private Bird owner;

    public override void Enter(Bird owner)
    {
        standardColor = owner.standardColorIdle;
        this.owner = owner;
    }

    public override void Execute(Bird owner)
    {
        // update forces

        // state change
        if (owner.Hungry && owner.Information.Count > 0)
            owner.ChangeState(owner.StateFeed);
    }

    public void FoundFood(Bird owner)
    {
        if (owner.Hungry)
        {
            owner.ChangeState(owner.StateFeed);
        }
        else if (owner.Informed)
        {
            owner.ChangeState(owner.StateCommunicate);
        }
    }

    public override void FoundBird(Transform bird)
    {
        owner.ChangeState(owner.StateCommunicate);
    }

    public override void Exit(Bird owner)
    {
        // fade out search state color
        owner.HighlightBird(standardColor);
    }
}
