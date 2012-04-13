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
    /// A pool of agents which can be used.
    /// </summary>
    public List<Agent<T>> Agents;
    /// <summary>
    /// a list of agents which are currently active
    /// </summary>
    public List<Agent<T>> ActiveAgents;

    /// <summary>
    /// a list of objects in the environment which can give information of type T
    /// </summary>
    public List<IOfferInformation<T>> InformationOwners;
}
