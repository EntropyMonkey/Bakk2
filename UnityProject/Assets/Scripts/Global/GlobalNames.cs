// GlobalNames.cs

using UnityEngine;

/// <summary>
/// All the constant strings used in this swarm implementation
/// </summary>
public class GlobalNames
{
    /// <summary>
    /// All tags used by birds
    /// </summary>
    public class Tags
    {
        public const string IOfferInformation = "IOfferInformation";
        public const string VisibleToBirds = "visibleToBirds";
    }

    /// <summary>
    /// The names for a bird's needs
    /// </summary>
    public class Needs
    {
        public const string Food = "food";
        public const string Information = "information";
    }

    /// <summary>
    /// The names for a bird's stats
    /// </summary>
    public class Stats
    {
        public const string Saturation = "saturation";
        public const string Information = "information";
    }
}
