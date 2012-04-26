// Environment.cs

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// The environment for agents, which manages where information can be gathered, a pool of agents and
/// which agents are active.
/// </summary>
/// <typeparam name="T">The type of information which can be gathered in this environment</typeparam>
public abstract class Environment<T> : MonoBehaviour where T : IInformation
{    
    /// <summary>
    /// All agents
    /// </summary>
    protected List<Agent<T>> agents;

    /// <summary>
    /// a list of objects in the environment which can give information of type T
    /// </summary>
    protected List<IOfferInformation<T>> informationOwners;
}
