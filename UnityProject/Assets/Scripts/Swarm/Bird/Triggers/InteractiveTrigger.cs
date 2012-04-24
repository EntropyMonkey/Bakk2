using UnityEngine;
using System.Collections;

public class InteractiveTrigger : BirdTrigger {

    void OnTriggerEnter(Collider other)
    {
        // if its a bird, try to enter communication state

        // if its food, try to enter eat state
    }

    void OnTriggerExit(Collider other)
    {
        // if its food, exit eat state
    }
}
