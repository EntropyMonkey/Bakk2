// Bird.cs

using UnityEngine;
using System.Collections;

/// <summary>
/// A bird can only gather information suited for birds.
/// </summary>
public class Bird : Agent<BirdInformation> {

    private FiniteStateMachine<Bird> FSM;

    private FSMState<Bird> stateSearch;
    private FSMState<Bird> stateCommunicate;
    private FSMState<Bird> stateIdle;

	void Start () {
	    
	}
	
	void Update () {
	
	}

    public override void GatherInformation(BirdInformation information)
    {
    }
}
