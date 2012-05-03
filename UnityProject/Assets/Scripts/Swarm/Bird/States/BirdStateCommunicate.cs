using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class BirdStateCommunicate : FSMState<Bird>
{
    /// <summary>
    /// stores the birds which will receive their answer next
    /// </summary>
    private Queue<CommunicationPartner> nextAnswerRecipientQueue;

    /// <summary>
    /// stores the communication partners of the bird, along with the start index and the current index
    /// of information
    /// </summary>
    private List<CommunicationPartner> communicationPartners;

    /// <summary>
    /// When this countdown reaches 0, the next answer recipient gets its answer
    /// </summary>
    private float answerCountdown = 0;

    /// <summary>
    /// A list of all partners whose startTimeout is still running
    /// </summary>
    private List<CommunicationPartner> countdownPartners;

    /// <summary>
    /// This state's owner
    /// </summary>
    private Bird owner;

    void OnEnable()
    {
        // add event handler
        Messenger<Bird>.AddListener(GlobalNames.Events.BirdEnteredInteractiveTrigger, Handshake);

        // initiate collections
        nextAnswerRecipientQueue = new Queue<CommunicationPartner>();
        communicationPartners = new List<CommunicationPartner>();
        countdownPartners = new List<CommunicationPartner>();
    }

    public override void Enter(Bird owner)
    {
        this.owner = owner;
    }

    public override void Execute(Bird owner)
    {
        // update starting countdowns
        for (int i = 0; i < countdownPartners.Count; i++)
        {
            countdownPartners[i].startTimeout -= Time.deltaTime;
            if (countdownPartners[i].startTimeout <= 0.0f)
            {
                communicationPartners.Add(countdownPartners[i]);
                countdownPartners.RemoveAt(i);

                // if it's the first bird being added to the communication partners, set the answer countdown
                if (answerCountdown <= 0)
                {
                    answerCountdown = owner.communicationSettings.Timeout;
                }
            }
        }

        // update communication
        if (answerCountdown > 0.0f)
        {
            answerCountdown -= Time.deltaTime;
            if (answerCountdown <= 0.0f)
            {
                CommunicationPartner next = nextAnswerRecipientQueue.Dequeue();
                next.bird.GatherInformation(owner.Information[next.currentIndex]);

                next.currentIndex++;
                // loop the index around
                if (next.currentIndex >= owner.Information.Count)
                    next.currentIndex = 0;
            }
        }
    }

    public override void Exit(Bird owner)
    {
        
    }

    private void Handshake(Bird other)
    {
        // store other in communicationPartner List
        int startInd = UnityEngine.Random.Range(0, owner.Information.Count - 1);
        CommunicationPartner newPartner = new CommunicationPartner
        {
            bird = other,
            startIndex = startInd,
            currentIndex = startInd == owner.Information.Count - 1 ? 0 : startInd,

            // determine which bird starts with asking by using a random (very short) timeout
            startTimeout = UnityEngine.Random.Range(0.0f, Time.deltaTime * 2.0f),
        };
        countdownPartners.Add(newPartner);
    }
}
