using UnityEngine;
using System;

public class MeasureData
{
    public float timestamp = -1;

    public int hungryBirds = 0;
    public int feedingBirds = 0;
    public int communicatingBirds = 0;
    public int exploringBirds = 0;

    public int discoveredFood = 0;
    public int spawnedFood = 0;
    public int removedFood = 0;
    public float averageDiscoveryDuration = 0; // Duration from spawning until discovery
    public float averageFoodLifetime = 0; // Average duration food is alive

    public float averageHops = 0;
    public float averageDuplicatesPerInformation = 0;
    public float averageInformationAge = 0;
    public float averageInformationCertainty = 0;

    public static bool operator >(MeasureData m1, MeasureData m2)
    {
        return m1.timestamp > m2.timestamp;
    }

    public static bool operator <(MeasureData m1, MeasureData m2)
    {
        return m1.timestamp < m2.timestamp;
    }

    public static bool operator ==(MeasureData m1, MeasureData m2)
    {
        return Mathf.Abs(m1.timestamp - m2.timestamp) < 0.001f;
    }

    public static bool operator !=(MeasureData m1, MeasureData m2)
    {
        return Mathf.Abs(m1.timestamp - m2.timestamp) > 0.001f;
    }

    public override bool Equals(object obj)
    {
        return (MeasureData)obj == this;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        string s = "";
        s += timestamp + "\n";

        s += hungryBirds + " ";
        s += feedingBirds + " ";
        s += communicatingBirds + " ";
        s += exploringBirds + "\n";

        s += discoveredFood + " ";
        s += spawnedFood + " ";
        s += removedFood + " ";
        s += averageDiscoveryDuration + " ";
        s += averageFoodLifetime + "\n";

        s += averageHops + " ";
        s += averageInformationAge + " ";
        s += averageInformationCertainty + " ";
        s += averageDuplicatesPerInformation;
        return s;
    }
}