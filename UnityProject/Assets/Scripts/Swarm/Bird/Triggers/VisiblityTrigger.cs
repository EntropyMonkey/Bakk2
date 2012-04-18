using UnityEngine;
using System.Collections;

public class VisiblityTrigger : BirdTrigger {

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == GlobalNames.Tags.VisibleToBirds)
        {
            owner.VisibleObjects.Add(other.transform.root.transform);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == GlobalNames.Tags.VisibleToBirds)
        {
            owner.VisibleObjects.Remove(other.transform.root.transform);
        }
    }
}
