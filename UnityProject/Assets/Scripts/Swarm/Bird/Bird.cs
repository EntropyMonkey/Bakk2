// Bird.cs

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// A bird can only gather information suited for birds.
/// </summary>
public class Bird : Agent<BirdInformation>
{
    #region Members
    #region Id
    // id
    private static int nextFreeId = 0;
    public int id;
    #endregion

    #region Debug
    // display values in inspector for debugging
    public float SaturationStat;
    public float InfoStat;
    #endregion

    #region State related
    // the state machine
    protected FiniteStateMachine<Bird> FSM;
    /// <summary>
    /// The bird's current state
    /// </summary>
    public FSMState<Bird> CurrentState { get { return FSM.CurrentState; } }

    /// <summary>
    /// Handles movement
    /// </summary>
    public BirdStateGlobal StateGlobal { get { return stateGlobal; } }
    private BirdStateGlobal stateGlobal;
    /// <summary>
    /// Handles exploring the environment
    /// </summary>
    public BirdStateExplore StateExplore { get { return stateExplore; } }
    private BirdStateExplore stateExplore;
    /// <summary>
    /// Handles communication with other birds
    /// </summary>
    public BirdStateCommunicate StateCommunicate { get { return stateCommunicate; } }
    private BirdStateCommunicate stateCommunicate;
    /// <summary>
    /// Handles feeding
    /// </summary>
    public BirdStateFeed StateFeed { get { return stateFeed; } }
    private BirdStateFeed stateFeed;

    /// <summary>
    /// whether the bird should ignore other birds
    /// </summary>
    public virtual bool ignoreBirds { get { return settings.ignoreBirds; } }

    public Vector3 targetPoint;
    public float outsideEnvironmentMultiplier = 0;

    private List<Transform> sightNeighbors;
    /// <summary>
    /// All currently visible birds are stored here
    /// </summary>
    public List<Transform> SightNeighbors { get { return sightNeighbors; } }

    private List<Transform> nearNeighbors;
    /// <summary>
    /// All currently nearly touching birds are stored here
    /// </summary>
    public List<Transform> NearNeighbors { get { return nearNeighbors; } }
    #endregion

    #region Needs
    /// <summary>
    /// A dictionary holding all the needs an agent has
    /// </summary>
    public Dictionary<string, float> Needs;

    public bool Hungry
    {
        get
        {
            return Needs[GlobalNames.Needs.Food] > Bird.settings.eatingThreshold;
        }
    }

    public bool Informed
    {
        get
        {
            return Needs[GlobalNames.Needs.Information] < Bird.settings.informationThreshold;
        }
    }
    #endregion

    #region Settings
    /// <summary>
    /// The bird's configurations
    /// </summary>
    public static BirdSettings settings;
    /// <summary>
    /// the most important settings in this project: the communication settings.
    /// They can be different for each bird (however, are only different for the three swarms)
    /// </summary>
    public BirdCommunicationSettings communicationSettings;
    #endregion

    #region Visualization
    // the color fading business
    public Color standardColorIdle;
    public Color standardColorCommunication; 
    public Color standardColorFeeding;
    public Color highlightInformationGathering;
    public Color highlightCommunication;
    private Color currentColor;
    public float blendingTime; // how long the blending needs (total)
    private float currentBlendingTime; // how long it has been blending up to now (state of the art)
    #endregion
    #endregion

    #region Methods
    #region Awake
    private void Awake ()
    {
        // set id
        id = nextFreeId++;

        // initialize lists
        Information = new List<BirdInformation>();
        sightNeighbors = new List<Transform>();
        nearNeighbors = new List<Transform>();

        // initialize states
        stateGlobal = ScriptableObject.CreateInstance<BirdStateGlobal>();
        stateCommunicate = ScriptableObject.CreateInstance<BirdStateCommunicate>();
        stateExplore = ScriptableObject.CreateInstance<BirdStateExplore>();
        stateFeed = ScriptableObject.CreateInstance<BirdStateFeed>();

        FSM = new FiniteStateMachine<Bird>();
        FSM.Configure(this, stateExplore, stateGlobal);

        // initialize needs
        Needs = new Dictionary<string, float>();
        Needs.Add(GlobalNames.Needs.Food, 1.0f);
        Needs.Add(GlobalNames.Needs.Information, 1.0f);

        gameObject.tag = GlobalNames.Tags.Bird;

        // set renderer colors
        renderer.material.color = currentColor = standardColorIdle;
	}

    private void Start()
    {

    }
    #endregion

    #region Update related
    private void Update () 
    {
        // update movement and state changes
        FSM.Update();

        // updates information age, deletes old info
        UpdateInformation();

        UpdateNeeds();

        // updates colors
        UpdateHighlighting();

        // show stats in inspector
        DebugUpdate();
	}
    
    /// <summary>
    /// Changing color at certain events
    /// </summary>
    private void UpdateHighlighting()
    {
        currentBlendingTime += Time.deltaTime;
        renderer.material.color = Color.Lerp(currentColor, ((IBirdState)FSM.CurrentState).standardColor, currentBlendingTime / blendingTime);
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
    }

    /// <summary>
    /// Updates the bird's needs
    /// </summary>
    private void UpdateNeeds()
    {
        Needs[GlobalNames.Needs.Information] =
            1.0f - Mathf.Max(Information.Count * settings.informationNeedSaturationPerInfo, 0.0f);

        Needs[GlobalNames.Needs.Food] += settings.maxFoodCapacity / settings.saturationDecreaseIn * Time.deltaTime;
        Needs[GlobalNames.Needs.Food] = Mathf.Min(Needs[GlobalNames.Needs.Food], 1.0f);
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
    #endregion

    #region HighlightBird
    /// <summary>
    /// Gives the birdmesh a new color for some time
    /// </summary>
    /// <param name="newColor">the new color</param>
    public void HighlightBird(Color newColor)
    {
        currentColor = newColor;
        currentBlendingTime = 0;
    }
    #endregion

    #region GatherInformation
    /// <summary>
    /// gathers the information if the circumstances allow it
    /// </summary>
    /// <param name="information">the information which will be gathered</param>
    public override void GatherInformation(BirdInformation information)
    {
        BirdInformation sameInfo = Information.Find(item => 
            item.MeasureEquality(information) < communicationSettings.equalityThreshold);
        
        if (information != null &&
            Needs[GlobalNames.Needs.Information] > settings.informationThreshold &&
            // only gather info if it is not yet in the list
            sameInfo == null)
        {
            HighlightBird(highlightInformationGathering);

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

            if (newInfo.certainty >= communicationSettings.certaintyThreshold)
            {
                Information.Add(newInfo);
            }

            UpdateInformation();

            // need to do that after information update
            if (newInfo.hops == 0 && newInfo.type == BirdInformation.BirdInformationType.FOOD)
            {
                ((IBirdState)FSM.CurrentState).FoundFood(newInfo.foodSourcePosition);
            }
        }
    }
    #endregion

    #region DiscreditFoodAt
    public void DiscreditFoodAt(Vector3 position)
    {
        BirdInformation wrongInfo = Information.Find(
            item => item.type == BirdInformation.BirdInformationType.FOOD && 
                Vector3.Distance(item.foodSourcePosition, position) <= 0.001f);

        if (wrongInfo != null)
        {
            Information.Remove(wrongInfo);
        }
    }
    #endregion

    #region ChangeState
    /// <summary>
    /// Changes the bird's state
    /// </summary>
    /// <param name="state">the new state</param>
    public virtual void ChangeState(FSMState<Bird> state)
    {
        FSM.ChangeState(state);
    }
    #endregion

    #region Neighbor related
    /// <summary>
    /// Adds a bird to the neighbor list
    /// </summary>
    /// <param name="birdTransform">the bird's transform</param>
    public void AddSightNeighbor(Transform birdTransform)
    {
        sightNeighbors.Add(birdTransform);
        ((IBirdState)FSM.CurrentState).FoundBird(birdTransform);
    }

    /// <summary>
    /// Removes the transform from the neighbor list
    /// </summary>
    public void RemoveSightNeighbor(Transform birdTransform)
    {
        sightNeighbors.Remove(birdTransform);
    }

    public void AddNearNeighbor(Transform birdTransform)
    {
        nearNeighbors.Add(birdTransform);
    }

    public void RemoveNearNeighbor(Transform birdTransform)
    {
        nearNeighbors.Remove(birdTransform);
    }
    #endregion

    #region Feed
    /// <summary>
    /// Eats as much as possible regarding bird's food capacity and amount of food ready for disposal
    /// </summary>
    /// <param name="food">The food source</param>
    public void Eat(Food food)
    {
        // eat until the bird is full
        float request = Needs[GlobalNames.Needs.Food] * settings.maxFoodCapacity;
        float meal = food.Eat(request);
        Needs[GlobalNames.Needs.Food] -= meal / settings.maxFoodCapacity;
        //Debug.Log("(" + id + ")Eating: \n" + "requested: " + request + " got: " + meal);
        GatherInformation(food.Information);
    }
    #endregion

    #region Communication related
    /// <summary>
    /// Gets a random piece of information from the other bird
    /// </summary>
    /// <param name="other">The communication partner</param>
    public virtual bool Communicate(Bird other)
    {
        if (!ignoreBirds)
        {
            ChangeState(StateCommunicate);
            StateCommunicate.AddCommunicationPartner(other);
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
        if (FSM.CurrentState == StateCommunicate)
        {
            StateCommunicate.GivenAllInfo(other);
        }
    }

    /// <summary>
    /// Aborts communications with the other bird
    /// </summary>
    /// <param name="other">the bird with which to abort communications</param>
    public void AbortCommunication(Bird other)
    {
        if (FSM.CurrentState == StateCommunicate)
        {
            StateCommunicate.RemoveCommunicationPartner(other);
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
            StateCommunicate.AddToInformationRecipients(other);
        }
    }
    #endregion
    #endregion

}
