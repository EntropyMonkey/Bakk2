using UnityEngine;
using System;

public struct MeasureData
{
    public float timestamp;
    public int hungryBirds;
    public int communicatingBirds;
    public int exploringBirds;
    public int discoveredFood;
    public int spawnedFood;
    public int eatenFood;

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
        s += timestamp + " ";
        s += hungryBirds + " ";
        s += communicatingBirds + " ";
        s += exploringBirds + " ";
        s += discoveredFood + " ";
        s += spawnedFood + " ";
        s += eatenFood;
        return s;
    }
}