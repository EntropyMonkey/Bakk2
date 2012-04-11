using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Agent : MonoBehaviour{
    
    protected List<IInformation> Information;

    protected bool alive;
    bool Alive { get { return alive; } }

    protected IPropagationSettings propSettings;
    public IPropagationSettings PropSettings { get { return propSettings; } }

    public void GatherInformation(IInformation information);
}
