using UnityEngine;
using System.Collections;

public class BirdTrigger : MonoBehaviour
{
    protected Bird owner;

    protected void Awake()
    {
        owner = transform.parent.gameObject.GetComponent<Bird>();
        Physics.IgnoreCollision(collider, GameObject.Find(GlobalNames.Names.Environment).collider);
    }

    //public abstract void AwakePhysics();
}
