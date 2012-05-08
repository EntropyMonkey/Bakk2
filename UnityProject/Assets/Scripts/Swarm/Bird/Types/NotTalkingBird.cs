using System;
using UnityEngine;

/// <summary>
/// This kind of bird cannot talk to other birds and needs to rely on watching them and on exploring
/// the environment on their own
/// </summary>
public class NotTalkingBird : Bird
{
    public override bool Communicate(Bird other)
    {
        other.AbortCommunication(this);
        return false;
    }
}
