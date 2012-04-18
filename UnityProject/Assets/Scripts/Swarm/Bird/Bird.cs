// Bird.cs

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// A bird can only gather information suited for birds.
/// </summary>
public class Bird : Agent<BirdInformation> {

    // the state machine
    private FiniteStateMachine<Bird> FSM;

    // all the states
    private BirdStateGlobal stateGlobal;
    private BirdStateSearch stateSearch;
    private BirdStateCommunicate stateCommunicate;
    private BirdStateIdle stateIdle;

    /// <summary>
    /// A dictionary holding all the needs an agent has
    /// </summary>
    public Dictionary<string, float> Needs;
    /// <summary>
    /// Current stats which influence the needs
    /// </summary>
    public Dictionary<string, float> Stats;

    /// <summary>
    /// A list of all visible objects
    /// </summary>
    public List<Transform> VisibleObjects { get; private set; }

	void Start ()
    {
        // initialize states
        stateGlobal = ScriptableObject.CreateInstance<BirdStateGlobal>();
        stateIdle = ScriptableObject.CreateInstance<BirdStateIdle>();
        stateCommunicate = ScriptableObject.CreateInstance<BirdStateCommunicate>();
        stateSearch = ScriptableObject.CreateInstance<BirdStateSearch>();

        FSM = new FiniteStateMachine<Bird>();
        FSM.Configure(this, stateIdle, stateGlobal);

        // initialize needs
        Needs.Add(GlobalNames.Needs.Food, 0.0f);
        Needs.Add(GlobalNames.Needs.Information, 1.0f);

        // initialize stats
        Stats.Add(GlobalNames.Stats.Saturation, 1.0f);
        Stats.Add(GlobalNames.Stats.Information, 0.0f);
	}
	
	void Update () 
    {
        FSM.Update();
	}

    public override void GatherInformation(BirdInformation information)
    {
    }

    public void ChangeState(FSMState<Bird> state)
    {
        FSM.ChangeState(state);
    }
}
