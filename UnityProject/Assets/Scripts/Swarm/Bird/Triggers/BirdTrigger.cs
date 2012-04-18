using UnityEngine;
using System.Collections;

public class BirdTrigger : MonoBehaviour {

    protected Bird owner;

	void Start () 
    {
        owner = transform.parent.gameObject.GetComponent<Bird>();
	}
}
