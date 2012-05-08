using UnityEngine;
using System.Collections;

public class InteractiveTrigger : BirdTrigger {

    new void Awake()
    {
        base.Awake();
        Physics.IgnoreCollision(collider, transform.parent.FindChild(GlobalNames.Names.NearTrigger).collider);
        Physics.IgnoreCollision(collider, transform.parent.FindChild(GlobalNames.Names.VisibleTrigger).collider);
    }

    void OnTriggerEnter(Collider other)
    {
        // if its food, try to enter eat state
        Food food = other.gameObject.GetComponent<Food>();
        if (food)
        {
            owner.Eat(food);
        }

        // if its a bird, try to enter communication state
        Bird bird = other.gameObject.GetComponent<Bird>();
        if (!owner.ignoreBirds && bird)
        {
            owner.Communicate(bird);
            bird.Communicate(owner);
        }
    }

    void OnTriggerStay(Collider other)
    {
    }

    void OnTriggerExit(Collider other)
    {
        // if its a bird, exit communication state
        Bird bird = other.gameObject.GetComponent<Bird>();
        if (!owner.ignoreBirds && bird)
        {
            owner.AbortCommunication(bird);
        }
    }
}
