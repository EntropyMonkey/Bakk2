// Agent.cs

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// An agent can be alive or not alive. It has certain settings for information propagation. It owns
/// a list of all the information it has already gathered.
/// </summary>
/// <typeparam name="T">The type of information which can be gathered by this agent</typeparam>
public abstract class Agent<T> : MonoBehaviour where T : IInformation
{
    /// <summary>
    /// A list holding all the information an agent has gathered.
    /// </summary>
    public List<T> Information;

    protected bool alive;
    /// <summary>
    /// Determines whether the agent is active and alive.
    /// </summary>
    bool Alive { get { return alive; } }

    protected IPropagationSettings propSettings;
    /// <summary>
    /// The information propagation settings of this agent.
    /// </summary>
    public IPropagationSettings PropSettings { get { return propSettings; } }

    /// <summary>
    /// This method determines how information is gathered and whether information is gathered at all.
    /// </summary>
    /// <param name="information"></param>
    public abstract void GatherInformation(T information);

    protected abstract void InitializeCommunicationSettings();
}
