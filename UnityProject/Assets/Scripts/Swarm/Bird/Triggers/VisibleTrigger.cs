using UnityEngine;
using System.Collections;

public class VisibleTrigger : BirdTrigger {
        
    new void Awake()
    {
        base.Awake();

        Physics.IgnoreCollision(collider, transform.parent.FindChild(GlobalNames.Names.InteractiveTrigger).collider);
        Physics.IgnoreCollision(collider, transform.parent.FindChild(GlobalNames.Names.NearTrigger).collider);    
    }

    void OnTriggerEnter(Collider other)
    {
        // if it offers information, gather it
        if (other.gameObject.tag == GlobalNames.Tags.IOfferInformation)
        {
            GenericComponents genericComponents = other.gameObject.GetComponent<GenericComponents>();
            if (genericComponents != null && genericComponents.iOfferBirdInformation != null)
            {
                owner.GatherInformation(genericComponents.iOfferBirdInformation.Information);

                if (genericComponents.iOfferBirdInformation.Information.type == BirdInformation.BirdInformationType.FOOD)
                {
                    other.gameObject.GetComponent<Food>().Eat(0);
                }
            }
        }
        
        // if it is a bird, get its position and velocity (observe)
        if (other.gameObject.tag == GlobalNames.Tags.Bird)
        {
            owner.AddSightNeighbor(other.transform);
        }
    }

    void OnTriggerExit(Collider other)
    {
        // if it is a bird, delete it from neighbors
        if (other.gameObject.tag == GlobalNames.Tags.Bird)
        {
            owner.RemoveSightNeighbor(other.transform);
        }
    }
}
