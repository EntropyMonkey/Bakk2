// Bird.cs

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// A bird can only gather information suited for birds.
/// </summary>
public class Bird : Agent<BirdInformation> {
    
    // id
    private static int nextFreeId = 0;
    public int id;

    // display values in inspector for debugging
    public float SaturationStat;
    public float InfoStat;

    // the state machine
    private FiniteStateMachine<Bird> FSM;

    // all the states
    private BirdStateGlobal stateGlobal;
    public BirdStateSearch StateSearch { get { return stateSearch; } }
    private BirdStateSearch stateSearch;
    public BirdStateCommunicate StateCommunicate { get { return stateCommunicate; } }
    private BirdStateCommunicate stateCommunicate;

    /// <summary>
    /// A dictionary holding all the needs an agent has
    /// </summary>
    public Dictionary<string, float> Needs;
    /// <summary>
    /// The bird's configurations
    /// </summary>
    public static BirdSettings settings;
    /// <summary>
    /// the most important settings in this project: the communication settings.
    /// They can be different for each bird (however, are only different for the three swarms)
    /// </summary>
    public BirdCommunicationSettings communicationSettings;

    /// <summary>
    /// All currently visible birds are stored here
    /// </summary>
    public List<Transform> Neighbors;

	private void Start ()
    {
        // set id
        id = nextFreeId++;

        // initialize lists
        Information = new List<BirdInformation>();
        Neighbors = new List<Transform>();

        // initialize states
        stateGlobal = ScriptableObject.CreateInstance<BirdStateGlobal>();
        stateCommunicate = ScriptableObject.CreateInstance<BirdStateCommunicate>();
        stateSearch = ScriptableObject.CreateInstance<BirdStateSearch>();

        FSM = new FiniteStateMachine<Bird>();
        FSM.Configure(this, stateSearch, stateGlobal);

        // initialize needs
        Needs = new Dictionary<string, float>();
        Needs.Add(GlobalNames.Needs.Food, 0.0f);
        Needs.Add(GlobalNames.Needs.Information, 1.0f);

        gameObject.tag = GlobalNames.Tags.Bird;

        // set communication settings here for debugging - delete when birds are created in environment
        communicationSettings.EqualityThreshold = 0.1f;
        communicationSettings.certaintyThreshold = 0.0f;
        communicationSettings.Timeout = 5.0f;
	}
	
	private void Update () 
    {
        FSM.Update();

        UpdateInformation();

        DebugUpdate();
	}

    private void DebugUpdate()
    {
        // for debugging purposes
        SaturationStat = 1.0f - Needs[GlobalNames.Needs.Food];
        InfoStat = 1.0f - Needs[GlobalNames.Needs.Information];
    }

    /// <summary>
    /// Updates stored information (certainty, needs)
    /// </summary>
    private void UpdateInformation()
    {
        // get significancy values, because they change with age and forget insignificant information
        for (int i = 0; i < Information.Count; i++)
        {
            if (CalculateCertainty(Information[i]) < communicationSettings.certaintyThreshold ||
                Information[i].age > settings.maxAge)
                Information.Remove(Information[i]);
        }

        Needs[GlobalNames.Needs.Information] = 
            1.0f - Mathf.Max(Information.Count * settings.informationNeedSaturationPerInfo, 0.0f);
    }

    /// <summary>
    /// Calculates the certainty for a piece of information
    /// </summary>
    /// <param name="info">the information, will not be changed</param>
    /// <returns>the certainty of the given information</returns>
    private float CalculateCertainty(BirdInformation info)
    {
        // calculate single certainties
        float hopCertainty = - (float)info.hops / (float)settings.maxHops + 1;
        float ageCertainty = - info.age / settings.maxAge + 1;
        // hop and age certainty are equally weighted - calculate average of all certainties
        return (hopCertainty + ageCertainty) * 0.5f;
    }

    /// <summary>
    /// gathers the information if the circumstances allow it
    /// </summary>
    /// <param name="information">the information which will be gathered</param>
    public override void GatherInformation(BirdInformation information)
    {
        BirdInformation sameInfo = Information.Find(item => 
            item.MeasureEquality(information) < communicationSettings.EqualityThreshold);
        
        if (information != null &&
            Needs[GlobalNames.Needs.Information] > settings.informationThreshold &&
            // only gather info if it is not yet in the list
            sameInfo == null)
        {
            BirdInformation newInfo = information.Copy();
            newInfo.gatheredTimestamp = Time.time;

            // gathering new information, no gossip, recall perfectly
            if (newInfo.hops == -1)
            {
                newInfo.firstSeenTimestamp = Time.time;
                newInfo.hops = 0;
            }
            else
            {
                newInfo.hops = information.hops + 1;
            }

            newInfo.certainty = CalculateCertainty(newInfo);

            // calculate significancy and decide whether to store this info
            float significancy = newInfo.certainty / communicationSettings.certaintyThreshold;

            if (significancy >= settings.significancyThreshold)
            {
                Information.Add(newInfo);
            }

            newInfo.DebugLogMe();
        }
    }

    /// <summary>
    /// Changes the bird's state
    /// </summary>
    /// <param name="state">the new state</param>
    public void ChangeState(FSMState<Bird> state)
    {
        FSM.ChangeState(state);
    }

    private void OnCollisionEnter(Collision collision)
    {
    }

    /// <summary>
    /// Eats as much as possible regarding bird's food capacity and amount of food ready for disposal
    /// </summary>
    /// <param name="food">The food source</param>
    public void Eat(Food food)
    {
        // does the bird want to eat?
        if (Needs[GlobalNames.Needs.Food] > settings.eatingThreshold)
        {
            // eat until the bird is full
            float request = Needs[GlobalNames.Needs.Food] * settings.maxFoodCapacity;
            float meal = food.Eat(request);
            Needs[GlobalNames.Needs.Food] -= meal / settings.maxFoodCapacity;
            Debug.Log("(" + id + ")Eating: \n" + "requested: " + request + " got: " + meal);
            GatherInformation(food.Information);
        }
    }

    /// <summary>
    /// Gets a random piece of information from the other bird
    /// </summary>
    /// <param name="other">The communication partner</param>
    public bool Communicate(Bird other)
    {
        // only if the bird is not too hungry
        if (Needs[GlobalNames.Needs.Food] <= settings.eatingThreshold)
        {
            ChangeState(stateCommunicate);
            stateCommunicate.AddCommunicationPartner(other);
            return true;
        }
        return false;
    }

    /// <summary>
    /// The other bird has given all the info it can
    /// </summary>
    /// <param name="other">the bird which has given all info</param>
    public void GivenAllInfo(Bird other)
    {
        if (FSM.CurrentState == stateCommunicate)
        {
            stateCommunicate.GivenAllInfo(other);
        }
    }

    /// <summary>
    /// Aborts communications with the other bird
    /// </summary>
    /// <param name="other">the bird with which to abort communications</param>
    public void AbortCommunication(Bird other)
    {
        if (FSM.CurrentState == stateCommunicate)
        {
            stateCommunicate.RemoveCommunicationPartner(other);
        }
    }

    /// <summary>
    /// Register for receiving a piece of info from the other bird
    /// </summary>
    /// <param name="other">the bird asking for information</param>
    public void AskForInformation(Bird other)
    {
        if (Communicate(other))
        {
            stateCommunicate.AddToInformationRecipients(other);
        }
    }
}
