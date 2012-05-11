using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Xml;

public class Measurement : MonoBehaviour
{
    public BirdEnvironmentSettings environmentSettings { get; set; }
    public BirdSettings birdSettings { get; set; }
    public BirdCommunicationSettings communicationSettings { get; set; }
    public FoodSettings foodSettings { get; set; }
    private Dictionary<string, BirdMovementSettings> movementSettings;

    private Dictionary<float, MeasureData> data;

    void Awake()
    {
        data = new Dictionary<float, MeasureData>();
        movementSettings = new Dictionary<string, BirdMovementSettings>();
    }

    public void AddMovementSettings(string name, BirdMovementSettings _movementSettings)
    {
        if (movementSettings == null)
        {
            movementSettings = new Dictionary<string, BirdMovementSettings>();
        }

        movementSettings.Add(name, _movementSettings);
    }

    public void AddData(MeasureData _data)
    {
        data.Add(_data.timestamp, _data);
    }

    /// <summary>
    /// Saves data in open office xml format
    /// </summary>
    public void SaveData()
    {
        // warning: ugly code. use with caution
        string path = @"D:\Dokumente\FH\Fächer\Semester 6\Bakk 2\Code\UnityProject\Assets\MeasureFiles\newFile0.txt";

        int i = 1;
        while (File.Exists(path))
        {
            path = path.Remove(path.Length - 5, 4);
            path += i + ".txt";
            i++;
        }

        using (System.IO.StreamWriter file = new System.IO.StreamWriter(path))
        {
            file.Write(environmentSettings.ToString());
            file.Write(birdSettings.ToString());
            file.Write(communicationSettings.ToString());

            foreach (KeyValuePair<string, BirdMovementSettings> kvp in movementSettings)
            {
                file.Write(kvp.Key + ":\n");
                file.Write(kvp.Value.ToString());
            }

            file.Write(foodSettings.ToString());

            file.Write("\n\n\n");

            for (i = 0; i < 14; i++)
            {
                switch (i)
                {
                    case 0:
                        file.Write("\ntimestamp\n");
                        break;
                    case 1:
                        file.Write("\nhungryBirds\n");
                        break;
                    case 2:
                        file.Write("\nfeedingBirds\n");
                        break;
                    case 3:
                        file.Write("\ncommunicatingBirds\n");
                        break;
                    case 4:
                        file.Write("\nexploringBirds\n");
                        break;
                    case 5:
                        file.Write("\ndiscoveredFood\n");
                        break;
                    case 6:
                        file.Write("\nspawnedFood\n");
                        break;
                    case 7:
                        file.Write("\nremovedFood\n");
                        break;
                    case 8:
                        file.Write("\naverageDiscoveryDuration\n");
                        break;
                    case 9:
                        file.Write("\naverageFoodLifeTime\n");
                        break;
                    case 10:
                        file.Write("\naverageHops\n");
                        break;
                    case 11:
                        file.Write("\naverageDuplicatesPerInformation\n");
                        break;
                    case 12:
                        file.Write("\naverageInformationAge\n");
                        break;
                    case 13:
                        file.Write("\naverageInformationCertainty\n");
                        break;
                }

                foreach (KeyValuePair<float, MeasureData> kvp in data)
                {
                    switch (i)
                    {
                        case 0:
                            file.Write(kvp.Value.timestamp + "\n");
                            break;
                        case 1:
                            file.Write(kvp.Value.hungryBirds + "\n");
                            break;
                        case 2:
                            file.Write(kvp.Value.feedingBirds + "\n");
                            break;
                        case 3:
                            file.Write(kvp.Value.communicatingBirds + "\n");
                            break;
                        case 4:
                            file.Write(kvp.Value.exploringBirds + "\n");
                            break;
                        case 5:
                            file.Write(kvp.Value.discoveredFood + "\n");
                            break;
                        case 6:
                            file.Write(kvp.Value.spawnedFood + "\n");
                            break;
                        case 7:
                            file.Write(kvp.Value.removedFood + "\n");
                            break;
                        case 8:
                            file.Write(kvp.Value.averageDiscoveryDuration + "\n");
                            break;
                        case 9:
                            file.Write(kvp.Value.averageFoodLifetime + "\n");
                            break;
                        case 10:
                            file.Write(kvp.Value.averageHops + "\n");
                            break;
                        case 11:
                            file.Write(kvp.Value.averageDuplicatesPerInformation + "\n");
                            break;
                        case 12:
                            file.Write(kvp.Value.averageInformationAge + "\n");
                            break;
                        case 13:
                            file.Write(kvp.Value.averageInformationCertainty + "\n");
                            break;
                    }
                }
            }
        }
    }
}