using UnityEngine;
using System.Collections;

/// <summary>
/// Get the generic components in the start function
/// </summary>
public class GenericComponents : MonoBehaviour {

    public IOfferInformation<BirdInformation> iOfferBirdInformation { private set; get; }

	// Use this for initialization
	void Start () 
    {
        iOfferBirdInformation = GetComponent(typeof(IOfferInformation<BirdInformation>)) as IOfferInformation<BirdInformation>;
	}
}
