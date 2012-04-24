using UnityEngine;
using System.Collections;

/// <summary>
/// An obstacle holds information which birds can obtain.
/// </summary>
public class Food : MonoBehaviour, IOfferInformation<BirdInformation> 
{
    public float TimeAlive = 10;

    public BirdInformation Information
    {
        get;
        set;
    }

    private float timeToLive;
    
	void Start () 
    {
        gameObject.tag = GlobalNames.Tags.IOfferInformation;

        // set timer
        timeToLive = TimeAlive;
	}
	
	void Update () 
    {
        timeToLive -= Time.deltaTime;
        if (timeToLive <= 0.0f)
            Destroy(gameObject);
	}

    void OnTriggerEnter(Collider other)
    {
    }
}
