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
    private BirdStateEat stateEat;

    /// <summary>
    /// A dictionary holding all the needs an agent has
    /// </summary>
    public Dictionary<string, float> Needs;
    /// <summary>
    /// Current stats which influence the needs
    /// </summary>
    public Dictionary<string, float> Stats;
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
        // initialize states
        stateGlobal = ScriptableObject.CreateInstance<BirdStateGlobal>();
        stateEat = ScriptableObject.CreateInstance<BirdStateEat>();
        stateCommunicate = ScriptableObject.CreateInstance<BirdStateCommunicate>();
        stateSearch = ScriptableObject.CreateInstance<BirdStateSearch>();

        FSM = new FiniteStateMachine<Bird>();
        FSM.Configure(this, stateSearch, stateGlobal);

        // initialize needs
        Needs = new Dictionary<string, float>();
        Needs.Add(GlobalNames.Needs.Food, 0.0f);
        Needs.Add(GlobalNames.Needs.Information, 1.0f);

        // initialize stats
        Stats = new Dictionary<string, float>();
        Stats.Add(GlobalNames.Stats.Saturation, 1.0f);
        Stats.Add(GlobalNames.Stats.Information, 0.0f);

        gameObject.tag = GlobalNames.Tags.Bird;
	}
	
	private void Update () 
    {
        FSM.Update();

        UpdateSignificancyValues();
	}

    private void UpdateSignificancyValues()
    {

    }

    public override void GatherInformation(BirdInformation information)
    {
        if (information != null && 
            // only gather info if it is not yet in the list
            Information.Find(
            item => item.MeasureEquality(information) > communicationSettings.EqualityThreshold) == null)
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

            // calculate new certainty
            float hopCertainty = 1 / (newInfo.hops / settings.maxHops);
            float ageCertainty = 1 / (newInfo.age / settings.maxAge);
            // hop and age certainty are equally weighted
            newInfo.certainty = (hopCertainty + ageCertainty) * 0.5f; 

            // calculate significancy and decide whether to store this info
            float significancy = newInfo.certainty / communicationSettings.CertaintyThreshold;

            if (significancy >= settings.significancyThreshold)
            {
                Information.Add(newInfo);
            }
        }
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
        // does the bird want to eat?
        if (Needs[GlobalNames.Needs.Food] > settings.eatingThreshold)
        {
            // eat until the bird is full
            float meal = food.Eat((1.0f - Stats[GlobalNames.Stats.Saturation]) * settings.maxFoodCapacity);
            Stats[GlobalNames.Stats.Saturation] += meal / settings.maxFoodCapacity;
            GatherInformation(food.Information);
        }
    }

    /// <summary>
    /// Gets a random piece of information from the other bird
    /// </summary>
    /// <param name="bird">The communication partner</param>
    public void Communicate(Bird bird)
    {
        communicationTimer -= Time.deltaTime;

        // does the bird want to gain info?
        if (communicationTimer <= 0.0f && 
            Needs[GlobalNames.Needs.Information] > settings.informationThreshold)
        {
            communicationTimer = communicationSettings.Timeout;
            GatherInformation(bird.AskForInformation());
        }
    }

    // returns a random piece of information gathered by this bird
    public BirdInformation AskForInformation()
    {
        if (Information.Count > 0)
            return Information[Random.Range(0, Information.Count - 1)];
        else
            return null;
    }
}
