using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Environment : MonoBehaviour {
    
    public List<Agent> Agents;
    public List<Agent> ActiveAgents;

    public List<IOfferInformation> InformationOwners;
}
