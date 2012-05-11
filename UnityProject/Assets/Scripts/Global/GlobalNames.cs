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
        public const string Bird = "Bird";
        public const string Environment = "Environment";
    }

    public class Names
    {
        public const string Environment = "Environment";
        public const string VisibleTrigger = "TRIGGER_Visible";
        public const string InteractiveTrigger = "TRIGGER_Interactive";
        public const string NearTrigger = "TRIGGER_Near";
        public const string Communication_MovementSettings = "Communication_MovementSettings";
        public const string Explore_MovementSettings = "Explore_MovementSettings";
        public const string Feed_MovementSettings = "Feed_MovementSettings";
    }

    /// <summary>
    /// The names for a bird's needs
    /// </summary>
    public class Needs
    {
        public const string Food = "food";
    }

    /// <summary>
    /// the names for the events
    /// </summary>
    public class Events
    {
        public const string UpdateMeasurements = "updateMeasurements";
    }
}
