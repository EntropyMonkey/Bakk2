using UnityEngine;
using System;

public class CommunicationPartner
{
    /// <summary>
    /// The communication partner
    /// </summary>
    public Bird bird;
    /// <summary>
    /// The index of the piece of information in the information list which will be the first to communicate
    /// </summary>
    public int startIndex;
    /// <summary>
    /// The current index of the piece of information
    /// </summary>
    public int currentIndex;
    /// <summary>
    /// The random start timeout which determines which bird starts asking for information.
    /// </summary>
    public float startTimeout;
}
