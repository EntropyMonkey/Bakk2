using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class BirdStateCommunicate : IBirdState
{
    /// <summary>
    /// stores the birds which will receive their answer next
    /// </summary>
    private ExtendedQueue<CommunicationPartner> nextAnswerRecipientQueue;

    /// <summary>
    /// stores the communication partners of the bird, along with the start index and the current index
    /// of information
    /// </summary>
    private List<CommunicationPartner> communicationPartners;

    /// <summary>
    /// The index of the partner in communication partners which will be asked next for info
    /// </summary>
    private int nextPartnerToAsk = 0;

    /// <summary>
    /// When this countdown reaches 0, the next answer recipient gets its answer
    /// </summary>
    private float answerCountdown = 0;

    /// <summary>
    /// This state's owner
    /// </summary>
    private Bird owner;

    public override void Enter(Bird owner)
    {
        this.owner = owner;

        // set color
        standardColor = owner.standardColorCommunication;

        // initiate collections (reset when entering state)
        nextAnswerRecipientQueue = new ExtendedQueue<CommunicationPartner>();
        communicationPartners = new List<CommunicationPartner>();

        cohesionMultiplier = 0.6f;
        separatingMultiplier = 0.4f;
        targetMultiplier = 0.0f;
        aligningMultiplier = 0.9f;

    }

    public override void Execute(Bird owner)
    {
        // update communication
        answerCountdown -= Time.deltaTime;
        if (answerCountdown <= 0.0f)
        {
            // if there are requests to answer, answer and then ask for info
            if (nextAnswerRecipientQueue.Count > 0)
            {
                CommunicationPartner next = nextAnswerRecipientQueue.Dequeue();

                // can only give info if there is some
                if (owner.Information.Count > 0)
                {
                    if (next.currentIndex >= owner.Information.Count)
                        next.currentIndex = 0;

                    next.bird.GatherInformation(owner.Information[next.currentIndex]);
                }
                else if (owner.Information.Count == 0)
                {
                    NoInformationToReceiveFromMe(communicationPartners[nextPartnerToAsk]);
                    // the count can change when calling the above function. If this is the case,
                    // the state has already exited and this method should not be executed any longer
                    if (communicationPartners.Count == 0)
                        return;
                }

                // ask the next partner for new information
                AskNextPartner();

                next.currentIndex++;
                // loop the index around
                if (next.currentIndex >= owner.Information.Count)
                {
                    next.currentIndex = 0;
                }

                if (next.currentIndex == next.startIndex)
                {
                    NoInformationToReceiveFromMe(next);
                }
                answerCountdown = Bird.settings.timeout;
            }
            // if there are no questions to answer
            else
            {
                // ask the next partner for new information
                AskNextPartner();
            }
        }

        // handle state change
        if (owner.Hungry && owner.Informed)
        {
            owner.ChangeState(owner.StateFeed);
        }
    }

    /// <summary>
    /// Tells the other partner that there is no more info this instance has to give it/him
    /// </summary>
    /// <param name="other">the partner to tell that there is nothing to get here</param>
    private void NoInformationToReceiveFromMe(CommunicationPartner other)
    {
        // tell the other bird that it has received all information this state's owner can
        // offer
        other.bird.GivenAllInfo(owner);
        other.receivedAllInfo = true;

        // if all information possible has been shared, dont talk to the other bird no more
        if (other.givenAllInfo)
        {
            RemoveCommunicationPartner(other);
        }
    }

    /// <summary>
    /// Sets that a bird has given all the information it can
    /// </summary>
    /// <param name="other">the bird which has given all its information</param>
    public void GivenAllInfo(Bird other)
    {
        CommunicationPartner partner;

        if ((partner = communicationPartners.Find(item => item.bird == other)) != null)
        {
            partner.givenAllInfo = true;

            // when there is no information left to share, remove the partner from the list
            if (partner.receivedAllInfo)
            {
                RemoveCommunicationPartner(partner);
            }

            // change state when there is no more information to get
            if (owner.Hungry && communicationPartners.Count == 0)
            {
                owner.ChangeState(owner.StateExplore);
            }
        }
    }

    /// <summary>
    /// Asks the next partner in the communicationPartners list for info
    /// </summary>
    private void AskNextPartner()
    {
        if (communicationPartners.Count > 0)
        {
            // register at the next partner to ask for receiving information
            communicationPartners[nextPartnerToAsk].bird.AskForInformation(owner);

            nextPartnerToAsk++;

            if (nextPartnerToAsk >= communicationPartners.Count)
                nextPartnerToAsk = 0;
        }
    }

    /// <summary>
    /// Adds a communication partner if it is not already in the list
    /// </summary>
    /// <param name="other">the bird to be added</param>
    public void AddCommunicationPartner(Bird other)
    {
        if (communicationPartners.Find(item => item.bird == other) == null)
        {
            int randomIndex = UnityEngine.Random.Range((int)0, (int)owner.Information.Count);
            communicationPartners.Add(new CommunicationPartner
            {
                bird = other,
                startIndex = randomIndex,
                currentIndex = randomIndex,
                // max timeout of estimatedly 3 frames
                startTimeout = UnityEngine.Random.Range(0.0f, Time.deltaTime * 3.0f)
            });
        }
    }

    /// <summary>
    /// Adds a bird which wants to receive information from this state's owner
    /// </summary>
    /// <param name="other">the bird requesting info</param>
    public void AddToInformationRecipients(Bird other)
    {
        if (communicationPartners.Count > 0)
        {
            nextAnswerRecipientQueue.Enqueue(communicationPartners.Find(item => item.bird == other));
        }
    }

    /// <summary>
    /// removes a communication partner
    /// </summary>
    /// <param name="other">the bird to remove</param>
    public void RemoveCommunicationPartner(Bird other)
    {
        CommunicationPartner partner;

        if (communicationPartners.Count > 0)
        {
            partner = communicationPartners.Find(item => item.bird == other);

            if (partner != null)
            {
                RemoveCommunicationPartner(partner);
            }
        }
    }

    private void RemoveCommunicationPartner(CommunicationPartner other)
    {
        if (communicationPartners.Count > 0)
        {
            // remove partner
            communicationPartners.Remove(other);
            if (nextAnswerRecipientQueue.Count > 0)
            {
                nextAnswerRecipientQueue.Remove(other);
            }

            // abort partner's communication with this state's owner
            other.bird.AbortCommunication(owner);


            // update the counter variable
            if (nextPartnerToAsk >= communicationPartners.Count)
                nextPartnerToAsk = 0;

            // if there are no partners left
            if (communicationPartners.Count == 0)
            {
                owner.ChangeState(owner.StateExplore);
            }
        }
    }

    /// <summary>
    /// Called when the state exits
    /// </summary>
    public override void Exit(Bird owner)
    {
        answerCountdown = 0;
        owner.HighlightBird(standardColor);
    }
}