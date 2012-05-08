using System;
using UnityEngine;
using System.Collections.Generic;

public class Measurement
{
    public BirdEnvironmentSettings environmentSettings { get; set; }
    public BirdSettings birdSettings { get; set; }
    public BirdCommunicationSettings communicationSettings { get; set; }
    public BirdMovementSettings movementSettings { get; set; }
    public FoodSettings foodSettings { get; set; }

    public List<MeasureData> data;
}