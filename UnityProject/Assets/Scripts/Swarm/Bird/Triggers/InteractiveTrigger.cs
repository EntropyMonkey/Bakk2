using UnityEngine;
using System.Collections;

public class InteractiveTrigger : BirdTrigger {

    void OnTriggerEnter(Collider other)
    {
        // if its food, try to enter eat state
        Food food = other.gameObject.GetComponent<Food>();
        if (food)
        {
            owner.Eat(food);
        }
    }

    void OnTriggerStay(Collider other)
    {
        // if its a bird, try to enter communication state
        Bird bird = other.gameObject.GetComponent<Bird>();
        if (bird)
        {
            owner.Communicate(bird);
        }
    }
}
