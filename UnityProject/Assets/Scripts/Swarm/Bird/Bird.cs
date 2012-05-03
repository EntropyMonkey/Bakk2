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
    private int id;

    // display values in inspector for debugging
    public float SaturationStat;
    public float InfoStat;

    // the state machine
    private FiniteStateMachine<Bird> FSM;

    // all the states
    private BirdStateGlobal stateGlobal;
    private BirdStateSearch stateSearch;
    private BirdStateCommunicate stateCommunicate;
    private BirdStateEat stateEat;

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

    private float communicationTimer = 0;

	private void Start ()
    {
        // set id
        id = nextFreeId++;

        // initialize lists
        Information = new List<BirdInformation>();
        Neighbors = new List<Transform>();

        // initialize states
        stateGlobal = ScriptableObject.CreateInstance<BirdStateGlobal>();
        stateEat = ScriptableObject.CreateInstance<BirdStateEat>();
        stateCommunicate = ScriptableObject.CreateInstance<BirdStateCommunicate>();
        stateSearch = ScriptableObject.CreateInstance<BirdStateSearch>();

        FSM = new FiniteStateMachine<Bird>();
        FSM.Configure(this, stateSearch, stateGlobal);

        // initialize needs
        Needs = new Dictionary<string, float>();
        Needs.Add(GlobalNames.Needs.Food, 1.0f);
        Needs.Add(GlobalNames.Needs.Information, 1.0f);

        gameObject.tag = GlobalNames.Tags.Bird;

        // set communication settings here for debugging - delete when birds are created in environment
        communicationSettings.EqualityThreshold = 0.1f;
        communicationSettings.certaintyThreshold = 0.0f;
        communicationSettings.Timeout = 0.0f;
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

    private void UpdateInformation()
    {
        // get significancy values, because they change with age and forget insignificant information
        for (int i = 0; i < Information.Count; i++)
        {
            if (CalculateCertainty(Information[i]) < communicationSettings.certaintyThreshold)
                Information.Remove(Information[i]);
        }

        Needs[GlobalNames.Needs.Information] = 
            1.0f - Mathf.Max(Information.Count * settings.informationNeedSaturationPerInfo, 0.0f);
    }

    public override void GatherInformation(BirdInformation information)
    {
        BirdInformation sameInfo = Information.Find(item => item.MeasureEquality(information) <
                communicationSettings.EqualityThreshold);

        Debug.Log("(" + id + ")GatherInformation:");
        if (sameInfo != null)
            Debug.Log("Found an identical item " + sameInfo.id);
        

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
        }
    }

    private float CalculateCertainty(BirdInformation info)
    {
        // calculate single certainties
        float hopCertainty = 1 / Mathf.Max(1 - info.hops / settings.maxHops, 0.000001f);
        float ageCertainty = 1 / Mathf.Max(1 - info.age / settings.maxAge, 0.000001f);
        // hop and age certainty are equally weighted
        return (hopCertainty + ageCertainty) * 0.5f;
    }

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
        Debug.Log("(" + id + ")Eating: ");
        // does the bird want to eat?
        if (Needs[GlobalNames.Needs.Food] > settings.eatingThreshold)
        {
            // eat until the bird is full
            float request = Needs[GlobalNames.Needs.Food] * settings.maxFoodCapacity;
            float meal = food.Eat(request);
            Needs[GlobalNames.Needs.Food] -= meal / settings.maxFoodCapacity;
            Debug.Log("requested: " + request + " got: " + meal);
            GatherInformation(food.Information);
        }
    }

    /// <summary>
    /// Gets a random piece of information from the other bird
    /// </summary>
    /// <param name="other">The communication partner</param>
    public void Communicate(Bird other)
    {
        communicationTimer -= Time.deltaTime;

        // does the bird want to gain info?
        if (communicationTimer <= 0.0f)
        {
            communicationTimer = communicationSettings.Timeout;
            GatherInformation(other.AskForInformation());
        }
    }

    // returns a random piece of information gathered by this bird
    public BirdInformation AskForInformation()
    {
        int randomNumber = Random.Range((int) 0, Information.Count);
        Debug.Log("(" + id + ") Giving info: " + randomNumber + " of " + Information.Count);
        if (Information.Count > 0)
            return Information[randomNumber];
        else
            return null;
    }
}
