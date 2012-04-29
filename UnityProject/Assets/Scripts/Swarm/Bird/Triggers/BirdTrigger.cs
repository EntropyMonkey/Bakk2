using UnityEngine;
using System.Collections;

public class BirdTrigger : MonoBehaviour
{
    protected Bird owner;

    protected void Start()
    {
        owner = transform.parent.gameObject.GetComponent<Bird>();
    }
}
