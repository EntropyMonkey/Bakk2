// IOfferInformation.cs

using UnityEngine;
using System.Collections;

/// <summary>
/// An object in the environment can offer information to agents. The agents decide what they do with
/// the information they could gather. An agent can only gather one type of information.
/// </summary>
/// <typeparam name="T">The type of information which can be gathered.</typeparam>
public interface IOfferInformation<T> where T : IInformation {
    /// <summary>
    /// The information which can be gathered here.
    /// </summary>
    T Information { get; set; }
}
