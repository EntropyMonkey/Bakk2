using UnityEngine;
using System.Collections;

public class NearTrigger : BirdTrigger {

    new void Awake()
    {
        base.Awake();
        Physics.IgnoreCollision(collider, transform.parent.FindChild(GlobalNames.Names.InteractiveTrigger).collider);
        Physics.IgnoreCollision(collider, transform.parent.FindChild(GlobalNames.Names.VisibleTrigger).collider);
    }

    void OnTriggerEnter(Collider other)
    {
        // if its a bird, try to enter communication state
        Bird bird = other.gameObject.GetComponent<Bird>();
        if (bird)
        {
            owner.AddNearNeighbor(bird.gameObject.transform);
        }
    }

    void OnTriggerStay(Collider other)
    {
    }

    void OnTriggerExit(Collider other)
    {
        // if its a bird, exit communication state
        Bird bird = other.gameObject.GetComponent<Bird>();
        if (bird)
        {
            owner.RemoveNearNeighbor(bird.gameObject.transform);
        }
    }
}
