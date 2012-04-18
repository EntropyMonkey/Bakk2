using UnityEngine;
using System.Collections;

/// <summary>
/// An obstacle holds information which birds can obtain.
/// </summary>
public class Obstacle : MonoBehaviour, IOfferInformation<BirdInformation> 
{
    public BirdInformation Information
    {
        get;
        set;
    }

	// Use this for initialization
	void Start () 
    {
        gameObject.tag = GlobalNames.Tags.IOfferInformation;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
