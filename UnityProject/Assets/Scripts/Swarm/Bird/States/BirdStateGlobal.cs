using UnityEngine;
using System;

public class BirdStateGlobal : FSMState<Bird>
{
    private Transform transform;
    /// <summary>
    /// The velocity created from the desire to reach the target point
    /// </summary>
    private Vector3 targetVelocity = Vector3.zero;
    /// <summary>
    /// the force keeping birds from colliding
    /// </summary>
    private Vector3 separatingForce = Vector3.zero;
    /// <summary>
    /// the force keeping birds together
    /// </summary>
    private Vector3 cohesionForce = Vector3.zero;
    /// <summary>
    /// the force created when leaving the environment
    /// </summary>
    private Vector3 stayInEnvironmentForce = Vector3.zero;
    /// <summary>
    /// the rigidbody's last frame velocity
    /// </summary>
    private Vector3 lastVelocity;

    public override void Enter(Bird owner)
    {
        transform = owner.gameObject.transform;
        lastVelocity = Vector3.forward * Bird.settings.maxVelocity;
    }

    public override void Execute(Bird owner)
    {
        Vector3 resultForce = lastVelocity;

        separatingForce = cohesionForce = targetVelocity = stayInEnvironmentForce = Vector3.zero;

        // calculate neighbor dependent forces
        if (owner.NearNeighbors.Count > 0)
        {
            foreach (Transform t in owner.NearNeighbors)
            {
                // calculate separating force
                separatingForce += transform.position - t.position;
            }
        }

        if (owner.SightNeighbors.Count > 0)
        {
            if (owner.NearNeighbors.Count > 0)
            {
                foreach (Transform t in owner.SightNeighbors)
                {
                    if (owner.NearNeighbors.Find(item => item == t) == null)
                    {
                        // calculate cohesionForce
                        cohesionForce += t.position - transform.position;
                    }
                }
            }
            else
            {
                foreach (Transform t in owner.SightNeighbors)
                {
                    // calculate cohesionForce
                    cohesionForce += t.position - transform.position;
                }
            }
        }

        // stay in environment
        if (owner.outsideEnvironmentMultiplier != 0)
        {
            stayInEnvironmentForce = BirdEnvironment.settings.bounds.center - transform.position;
        }

        // calculate velocity to go to target point
        targetVelocity = owner.targetPoint - transform.position;

        // add forces
        resultForce += 
            separatingForce * ((IBirdState)owner.CurrentState).movementSettings.separatingMultiplier * Time.deltaTime +
            cohesionForce * ((IBirdState)owner.CurrentState).movementSettings.cohesionMultiplier * Time.deltaTime +
            targetVelocity * ((IBirdState)owner.CurrentState).movementSettings.targetMultiplier * Time.deltaTime +
            stayInEnvironmentForce * owner.outsideEnvironmentMultiplier * Time.deltaTime;

        if (owner.SightNeighbors.Count > 0)
        {
            Vector3 heading = resultForce;
            // average the heading with neighbors
            foreach (Transform neighbor in owner.SightNeighbors)
            {
                heading += neighbor.gameObject.rigidbody.velocity;
            }
            heading /= owner.SightNeighbors.Count;
            resultForce = heading.normalized * resultForce.magnitude;
        }

        resultForce = resultForce.normalized * Mathf.Min(resultForce.magnitude, Bird.settings.maxVelocity);
        resultForce = resultForce.normalized * Mathf.Max(resultForce.magnitude, Bird.settings.minVelocity);

        transform.position += resultForce * Time.deltaTime;

        lastVelocity = resultForce;
    }

    public override void Exit(Bird owner)
    {

    }

    /// <summary>
    /// randomly changes direction
    /// </summary>
    public void ChangeDirection()
    {
        float speed = lastVelocity.magnitude;

        Vector3 randomDirection = Vector3.zero;
        randomDirection.x = UnityEngine.Random.Range(0.0f, 10.0f);
        randomDirection.y = UnityEngine.Random.Range(0.0f, 10.0f);
        randomDirection.z = UnityEngine.Random.Range(0.0f, 10.0f);
        lastVelocity = randomDirection.normalized * speed;
    }
}
